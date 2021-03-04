using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class LinearProjectile : BaseBehaviour, IPrefabPool
{
    private const string projectileSpawnTransform = "ProjectileSpawnTransform";

    
    public override BaseBehaviourData Data { get => data; set => data = (LinearProjectileData)value; }
	[SerializeField] protected new LinearProjectileData data;


	public int PoolSize => data.poolSize;
    private SyncList<uint> pool = new SyncList<uint>();
	public SyncList<uint> Pool => pool;

	public GameObject Prefab => data.projectilePrefab;



	/* TODO: make this an IRotationModifier and on enter add it to the players rotationinput, on tick update to mouseposition, on exit remove from RotationHandler */

	public override void Initialize()
	{
        if (hasAuthority) InitializePool();
	}

    public override void OnExit(BaseAbilityState.AbilityStateContext ctx)
    {
        if (ctx.stateCompleted) OnStateCompleted();
    }

    private void OnStateCompleted()
    {
        Transform spawn = transform.parent.transform.Find(projectileSpawnTransform);

        if (spawn == null)
        {
            Debug.Log("No spawning Position could be located for a projectile.");
            return;
        }

        GameObject instance = SpawnPrefab(spawn.position, spawn.rotation);

        if (instance == null) throw new Exception("No instance could be fetched from pool.");

        Projectile projectile = instance.GetComponent<Projectile>();

        Vector3 mousePosition = PlayerInputHandler.GetMousePositionWorldSpace();

        projectile.LifeTimeEnded += OnLifeTimeEnded;
        projectile.TargetHit += OnTargetHit;

        /* TODO: this currently works, but if it breaks in future it might be because the projectile gets enabled over a clientrpc
                 and only then fired. in the onenable method of the projectile the lifetime coroutine is started if it is fired
                 and it is only fired because the delay of the clientrps call */

        projectile.Fire( Vector3.ProjectOnPlane(
            mousePosition - instance.transform.position,
            Vector3.up
            ));
    }

	public void InitializePool()
	{
		for (int i = 0; i < PoolSize; i++)
        {
            CmdSpawnPrefab();
        }
	}

    [Command(ignoreAuthority = true)]
    private void CmdSpawnPrefab()
    {
        GameObject instance = Instantiate(Prefab, Vector3.zero, Quaternion.identity);

        NetworkServer.Spawn(instance, transform.parent.GetComponent<NetworkIdentity>().connectionToClient);

        uint netId = instance.GetComponent<NetworkIdentity>().netId;

        RpcSetInstanceActive(netId, false);
        RpcInitializeInstance(netId);

        Pool.Add(netId);
    }

    [ClientRpc]
    private void RpcInitializeInstance(uint netId)
    {
        NetworkIdentity.spawned[netId].GetComponent<Projectile>().Initialize(data.movementSpeed, data.lifeTime, transform.parent.GetComponent<NetworkIdentity>().netId);
    }

	public GameObject GetFromPool()
	{
        foreach(uint netId in Pool)
        {
            GameObject instance = NetworkIdentity.spawned[netId].gameObject;

            if (!instance.activeInHierarchy)
            {
                return instance;
            }
        }

        Debug.Log("No instance of " + Prefab.name + " is currently available.");
        return null;
	}

	public GameObject SpawnPrefab(Vector3 position, Quaternion rotation)
	{
        GameObject instance = GetFromPool();

        if (instance == null) return null;

        Debug.Log("Grabbing " + Prefab.name + " from pool.");

        instance.transform.position = position;
        instance.transform.rotation = rotation;

        CmdSetInstanceActive(instance.GetComponent<NetworkIdentity>().netId, true);

        return instance;
	}

	[Command]
	private void CmdSetInstanceActive(uint netId, bool active) => RpcSetInstanceActive(netId, active);

    [ClientRpc]
    private void RpcSetInstanceActive(uint netId, bool active) => NetworkIdentity.spawned[netId].gameObject.SetActive(active);

	public void UnspawnPrefab(GameObject instance)
	{
        Debug.Log("Re-pooling " + instance.name);

        /* TODO: unsubscribing might not be necessary, if subscription is done in CmdSpawnPrefab */
        Projectile projectile = instance.GetComponent<Projectile>();

        projectile.LifeTimeEnded -= OnLifeTimeEnded;
        projectile.TargetHit -= OnTargetHit;

        CmdSetInstanceActive(instance.GetComponent<NetworkIdentity>().netId, false);

        instance.transform.position = Vector3.zero;
        instance.transform.rotation = Quaternion.identity;
	}
    
    private void OnTargetHit(Projectile projectile, GameObject other)
    {
        GetComponent<Ability>().SignalTargetHit(projectile.gameObject, other);

        UnspawnPrefab(projectile.gameObject);
    }

    private void OnLifeTimeEnded(Projectile projectile)
    {
        UnspawnPrefab(projectile.gameObject);
    }

}
