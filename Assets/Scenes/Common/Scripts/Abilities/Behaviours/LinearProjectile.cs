using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class LinearProjectile : BaseBehaviour
{
    [SerializeField] GameObject projectilePrefab = null;

    [Header("Projectile Settings")]
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float lifeTime = 3f;

    private AbilityStateMachine stateMachine = null;
    private const string projectileSpawnTransform = "ProjectileSpawnTransform";

    private List<Projectile> projectileInstances = new List<Projectile>();
    
    public LinearProjectile(AbilityStateMachine stateMachine)
    {
        executionMask = ExecutionMask.CASTING;

        this.stateMachine = stateMachine;
    }

    public override void Tick() {}

    public override void OnEnter()
    {
        stateMachine.StateCompleted += OnStateCompleted; 
    }

    public override void OnExit()
    {
        stateMachine.StateCompleted -= OnStateCompleted;
    }

    private void OnStateCompleted()
    {
        GameObject projectileInstance = CmdRequestProjectileSpawn();


        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.Initialize(movementSpeed, lifeTime);

        Vector3 mousePosition = PlayerInputHandler.GetMousePositionWorldSpace();

        projectile.LifeTimeEnded += OnLifeTimeEnded;
        projectile.TargetHit += OnTargetHit;

        projectile.Fire(projectileInstance.transform.position - mousePosition);
    }

    [Command]
    private GameObject CmdRequestProjectileSpawn()
    {
        Transform spawn = stateMachine.owner.transform.Find(projectileSpawnTransform);

        if (spawn == null)
        {
            Debug.Log("No spawning Position could be located for a projectile.");
            return null;
        }

        GameObject projectileInstance = GameObject.Instantiate(projectilePrefab, spawn.position, spawn.rotation);
        NetworkServer.Spawn(projectileInstance);

        return projectileInstance;
    }

    private void OnTargetHit(Projectile projectile, GameObject other)
    {
        // explode projectile
    }

    private void OnLifeTimeEnded(Projectile projectile)
    {
        Projectile.Destroy(projectile.gameObject);
    }


    public override object Clone()
    {
        return new LinearProjectile(stateMachine);
    }
}
