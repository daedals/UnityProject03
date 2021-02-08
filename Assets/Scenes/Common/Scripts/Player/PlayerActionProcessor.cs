using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerActionProcessor : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _projectilePrefab = null;
    [SerializeField] GameObject _projectileSpawn = null;

    public event Action OnProjectileSpawned;

    private Projectile _lastSpawnedProjectile = null;

	public override void OnStartAuthority()
	{
        enabled = true;

        PlayerInputHandler.OnLMB += SetLMBState;
	}

    #region Client

    [Client]
    private void SetLMBState(bool lmbState)
    {
        if(lmbState) SpawnProjectile();
        else CmdFireProjectile();
    }

    [Command]
    private void CmdSpawnProjectile() => SpawnProjectile();

    [Command]
    private void CmdFireProjectile() => FireProjectile();

    #endregion

    #region Server

    [Server]
    private void SpawnProjectile()
    {
        GameObject projectileInstance = Instantiate(
            _projectilePrefab, 
            _projectileSpawn.transform.position, 
            _projectileSpawn.transform.rotation);

        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.SetReferences(_projectileSpawn, this);

        NetworkServer.Spawn(projectileInstance);

        _lastSpawnedProjectile = projectile;

        OnProjectileSpawned?.Invoke();
    }

    private void FireProjectile()
    {
        _lastSpawnedProjectile.Fire();
    }

    #endregion
}
