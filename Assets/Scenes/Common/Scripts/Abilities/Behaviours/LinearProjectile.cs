using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public class LinearProjectile : BaseBehaviour
{
    private const string projectileSpawnTransform = "ProjectileSpawnTransform";

    private List<Projectile> projectileInstances = new List<Projectile>();


    /*
    // public new LinearProjectileData Data { get; protected set; }

    everytime we use a derived BaseBehaviourData (such as LinearProjectileData) we have to cast to that inheriting class
    this is because c# doe not support return type covariance (to overwrite local Data property to the derived class)
    */
	public LinearProjectile(LinearProjectileData data) : base(data) {}

    /* TODO: make this an IRotationModifier and on enter add it to the players rotationinput, on tick update to mouseposition, on exit remove from RotationHandler */

    [Client]
    public override void OnExit(BaseAbilityState.AbilityStateContext ctx)
    {
        if (ctx.stateCompleted) OnStateCompleted();
    }

    [Client]
    private void OnStateCompleted()
    {
        GameObject projectileInstance = CmdRequestProjectileSpawn();

        // LinearProjectileData Data = (LinearProjectileData)this.Data;

        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.Initialize(((LinearProjectileData)Data).movementSpeed, ((LinearProjectileData)Data).lifeTime);

        Vector3 mousePosition = PlayerInputHandler.GetMousePositionWorldSpace();

        projectile.LifeTimeEnded += OnLifeTimeEnded;
        projectile.TargetHit += OnTargetHit;

        projectile.Fire( Vector3.ProjectOnPlane(
            mousePosition - projectileInstance.transform.position,
            Vector3.up
            ));
        Debug.Log("Shooting bullet.");
    }

    [Command]
    private GameObject CmdRequestProjectileSpawn()
    {
        Transform spawn = ability.owner.transform.Find(projectileSpawnTransform);

        if (spawn == null)
        {
            Debug.Log("No spawning Position could be located for a projectile.");
            return null;
        }

        GameObject projectileInstance = GameObject.Instantiate(((LinearProjectileData)Data).projectilePrefab, spawn.position, spawn.rotation);
        NetworkServer.Spawn(projectileInstance);

        return projectileInstance;
    }

    [Client]
    private void OnTargetHit(Projectile projectile, GameObject other)
    {
        // explode projectile
        ability.SignalTargetHit(other);

        CmdDestroyProjectile(projectile);
    }

    [Client]
    private void OnLifeTimeEnded(Projectile projectile)
    {
        CmdDestroyProjectile(projectile);
    }

    [Command]
    private void CmdDestroyProjectile(Projectile projectile)
    {
        Projectile.Destroy(projectile.gameObject);
    }


    public override object Clone()
    {
        return new LinearProjectile((LinearProjectileData)Data);
    }
}
