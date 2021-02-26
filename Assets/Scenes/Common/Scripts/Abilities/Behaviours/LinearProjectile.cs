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

    // private SyncList<GameObject> pool = new SyncList<GameObject>();
	// public SyncList<GameObject> Pool => pool;
    private List<GameObject> pool = new List<GameObject>();
	public List<GameObject> Pool => pool;

	public GameObject Prefab => data.projectilePrefab;



	/* TODO: make this an IRotationModifier and on enter add it to the players rotationinput, on tick update to mouseposition, on exit remove from RotationHandler */

	public override void Initialize(Ability ability)
	{
		base.Initialize(ability);

        // assign client authority to be able to send commands to server (and spawn projectiles)
        // GetComponent<NetworkIdentity>().AssignClientAuthority(ability.owner.GetComponent<NetworkIdentity>().connectionToClient);

        InitializePool();
	}

    public override void OnExit(BaseAbilityState.AbilityStateContext ctx)
    {
        if (ctx.stateCompleted) OnStateCompleted();
    }

    private void OnStateCompleted()
    {
        Transform spawn = ability.owner.transform.Find(projectileSpawnTransform);

        if (spawn == null)
        {
            Debug.Log("No spawning Position could be located for a projectile.");
            return;
        }

        GameObject instance = SpawnPrefab(spawn.position, spawn.rotation);

        if (instance == null) throw new Exception("No instance could be fetched from pool.");

        Projectile projectile = instance.GetComponent<Projectile>();
        projectile.Initialize(data.movementSpeed, data.lifeTime);

        Vector3 mousePosition = PlayerInputHandler.GetMousePositionWorldSpace();

        projectile.LifeTimeEnded += OnLifeTimeEnded;
        projectile.TargetHit += OnTargetHit;

        projectile.Fire( Vector3.ProjectOnPlane(
            mousePosition - instance.transform.position,
            Vector3.up
            ));
    }

    private void OnTargetHit(Projectile projectile, GameObject other)
    {
        // TODO: explode projectile
        ability.SignalTargetHit(projectile.gameObject, other);

        // RequestInstanceDespawn(projectile.Ticket);
    }

    private void OnLifeTimeEnded(Projectile projectile)
    {
        UnspawnPrefab(projectile.gameObject);
    }

	public void InitializePool()
	{
		for (int i = 0; i < PoolSize; i++)
        {
            CmdSpawnPrefab();
        }
	}

    [Command]
    private void CmdSpawnPrefab()
    {
        GameObject instance = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
        instance.SetActive(false);
        NetworkServer.Spawn(instance, ability.owner.GetComponent<NetworkIdentity>().connectionToClient);

        Pool.Add(instance);
    }

	public GameObject GetFromPool()
	{
        foreach(GameObject instance in Pool)
        {
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

        instance.SetActive(true);

        return instance;
	}

	public void UnspawnPrefab(GameObject instance)
	{
        Debug.Log("Re-pooling " + instance.name);

        /* TODO: unsubscribing might not be necessary, if subscription is done in CmdSpawnPrefab */
        Projectile projectile = instance.GetComponent<Projectile>();

        projectile.LifeTimeEnded -= OnLifeTimeEnded;
        projectile.TargetHit -= OnTargetHit;

        instance.SetActive(false);

        instance.transform.position = Vector3.zero;
        instance.transform.rotation = Quaternion.identity;
	}
}
