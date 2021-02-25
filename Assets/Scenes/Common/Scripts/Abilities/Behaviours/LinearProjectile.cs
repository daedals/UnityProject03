using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class LinearProjectile : BaseBehaviour, ISpawnRequester
{
    private const string projectileSpawnTransform = "ProjectileSpawnTransform";

    private List<Projectile> projectileInstances = new List<Projectile>();

	public ulong Id { get; set; }
	// public List<Tuple<int, GameObject>> SpawnedInstances 
    // {
    //     // TODO: change for more protection
    //     get; 
    //     set; 
    // }

	/*
    // public new LinearProjectileData Data { get; protected set; }

    everytime we use a derived BaseBehaviourData (such as LinearProjectileData) we have to cast to that inheriting class
    this is because c# doe not support return type covariance (to overwrite local Data property to the derived class)
    */
    
	// public LinearProjectile(LinearProjectileData data) : base(data) {}

	/* TODO: make this an IRotationModifier and on enter add it to the players rotationinput, on tick update to mouseposition, on exit remove from RotationHandler */

	public override void Initialize(Ability ability)
	{
		base.Initialize(ability);

        ability.owner.GetComponent<PlayerPrefabPool>().Add(this, ((LinearProjectileData)Data).projectilePrefab);
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

        PlayerPrefabPool pool = ability.owner.GetComponent<PlayerPrefabPool>();

        int ticket = pool.RequestPrefabSpawn(
            this, 
            spawn.position,
            spawn.rotation);

        pool.StartCoroutine(FetchInstance(ticket));
    }

    private IEnumerator FetchInstance(int ticket)
    {
        System.Tuple<ulong, GameObject> tuple;

        while (true)
        {
            // TODO: put in other break condition on failure (times or counted)
            tuple = ability.owner.GetComponent<PlayerPrefabPool>().RequestSpawnedInstance(this, ticket);

            if (tuple != null) break;

            Debug.Log("Could not fetch requested instance.");
            yield return 0;
        }

        Debug.Log("Successfully fetched instance.");

        System.Tuple<int, GameObject> instance = new Tuple<int, GameObject>(ticket, tuple.Item2);

        // SpawnedInstances.Add(instance);

        OnRequestedInstanceReceived(instance);
    }

    public void OnRequestedInstanceReceived(System.Tuple<int, GameObject> instance)
    {
        Projectile projectile = instance.Item2.GetComponent<Projectile>();
        projectile.Initialize(instance.Item1, ((LinearProjectileData)Data).movementSpeed, ((LinearProjectileData)Data).lifeTime);

        Vector3 mousePosition = PlayerInputHandler.GetMousePositionWorldSpace();

        projectile.LifeTimeEnded += OnLifeTimeEnded;
        projectile.TargetHit += OnTargetHit;

        projectile.Fire( Vector3.ProjectOnPlane(
            mousePosition - instance.Item2.transform.position,
            Vector3.up
            ));
        Debug.Log("Shooting bullet.");
    }

    private void OnTargetHit(Projectile projectile, GameObject other)
    {
        // TODO: explode projectile
        ability.SignalTargetHit(projectile.gameObject, other);

        // RequestInstanceDespawn(projectile.Ticket);
    }

    private void OnLifeTimeEnded(Projectile projectile)
    {
        RequestInstanceDespawn(projectile.Ticket);
    }

    private void RequestInstanceDespawn(int ticket) => ability.owner.GetComponent<PlayerPrefabPool>().RequestInstanceDespawn(this, ticket);
}
