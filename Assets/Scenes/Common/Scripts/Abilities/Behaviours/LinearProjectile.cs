using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class LinearProjectile : BaseBehaviour, IPrefabPool
{
    private const string projectileSpawnTransform = "ProjectileSpawnTransform";

    
    public override BaseBehaviourData Data { get => data; set => data = (LinearProjectileData)value; }
	[SerializeField] private LinearProjectileData data;

	public override void Initialize()
	{
        if (hasAuthority) InitializePool();
	}


#region IPrefabPool

	public int PoolSize => data.poolSize;
    private SyncList<uint> pool = new SyncList<uint>();
	public SyncList<uint> Pool => pool;
	public GameObject Prefab => data.projectilePrefab;
    
	public void InitializePool()
	{
		for (int i = 0; i < PoolSize; i++)
        {
            CmdSpawnPrefab();
        }
	}

    [Command(requiresAuthority = false)]
    private void CmdSpawnPrefab()
    {
        GameObject instance = Instantiate(Prefab, Vector3.zero, Quaternion.identity);

        NetworkServer.Spawn(instance, transform.parent.GetComponent<NetworkIdentity>().connectionToClient);

        uint netId = instance.GetComponent<NetworkIdentity>().netId;

        SetInstanceActive(netId, false);
        RpcSetInstanceActive(netId, false);

        InitializeInstance(netId, transform.parent.GetComponent<NetworkIdentity>().netId);
        RpcInitializeInstance(netId, transform.parent.GetComponent<NetworkIdentity>().netId);

        Pool.Add(netId);
    }

    [ClientRpc]
    private void RpcInitializeInstance(uint netId, uint playerNetId) => InitializeInstance(netId, playerNetId);

    private void InitializeInstance(uint netId, uint playerNetId)
    {
        if (NetworkIdentity.spawned.TryGetValue(netId, out NetworkIdentity projectile))
        {
            projectile.GetComponent<Projectile>().Initialize(data.movementSpeed, data.lifeTime, playerNetId);
        }
        else throw new Exception("Projectile has not spawned yet.");
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

        /* TODO: unsubscribing might not be necessary, if subscription is done in CmdSpawnPrefab */
        Projectile projectile = instance.GetComponent<Projectile>();

        projectile.LifeTimeEnded += OnLifeTimeEnded;
        projectile.TargetHit += OnTargetHit;

        Debug.Log("Grabbing " + Prefab.name + " from pool.");

        uint netId = instance.GetComponent<NetworkIdentity>().netId;

        CmdSetProjectileTransform(netId, position, rotation);

        Vector3 mousePosition = PlayerInputHandler.GetMousePositionWorldSpace();

        projectile.Fire( Vector3.ProjectOnPlane(
            mousePosition - position,
            Vector3.up
            ));

        CmdSetInstanceActive(netId, true);

        return instance;
	}

    [Command]
    private void CmdSetProjectileTransform(uint netId, Vector3 position, Quaternion rotation)
    {
        SetProjectileTransform(netId, position, rotation);
        RpcSetProjectileTransform(netId, position, rotation);
    }
    
    [ClientRpc]
    private void RpcSetProjectileTransform(uint netId, Vector3 position, Quaternion rotation) => SetProjectileTransform(netId, position, rotation);

    private void SetProjectileTransform(uint netId, Vector3 position, Quaternion rotation)
    {
        if (NetworkIdentity.spawned.TryGetValue(netId, out NetworkIdentity projectile))
        {
            projectile.gameObject.transform.position = position;
            projectile.gameObject.transform.rotation = rotation;
        }
        else throw new Exception("Projectile has not spawned yet.");
    }


	[Command]
	private void CmdSetInstanceActive(uint netId, bool active)
    {
        SetInstanceActive(netId, active);
        RpcSetInstanceActive(netId, active);
    }

    [ClientRpc]
    private void RpcSetInstanceActive(uint netId, bool active) => SetInstanceActive(netId, active);

    private void SetInstanceActive(uint netId, bool active)
    {
        if (NetworkIdentity.spawned.TryGetValue(netId, out NetworkIdentity projectile))
        {
            projectile.gameObject.SetActive(active);
        }
        else throw new Exception("Projectile has not spawned yet.");
    }

	public void UnspawnPrefab(GameObject instance)
	{
        Debug.Log("Re-pooling " + instance.name);

        CmdSetInstanceActive(instance.GetComponent<NetworkIdentity>().netId, false);

        instance.transform.position = Vector3.zero;
        instance.transform.rotation = Quaternion.identity;
	}

#endregion

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
    }
    
    private void OnTargetHit(Projectile projectile, GameObject other)
    {
        GetComponent<MecanimAbility>().SignalTargetHit(projectile.gameObject, other);

        UnspawnPrefab(projectile.gameObject);
    }

    private void OnLifeTimeEnded(Projectile projectile)
    {
        UnspawnPrefab(projectile.gameObject);
    }

}
