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
    public event Action OnProjectileFired;

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
        else FireProjectile();
    }

    private void FireProjectile() => OnProjectileFired?.Invoke();

    [Command]
    private void CmdSpawnProjectile() => SpawnProjectile();

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

        OnProjectileSpawned?.Invoke();
    }

    #endregion
}
