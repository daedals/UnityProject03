using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerActionProcessor : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _projectileSpawn = null;

    public event Action<PlayerActionProcessor> OnProjectileSpawnRequested;
    public event Action<PlayerActionProcessor> OnProjectileFireRequested;

    public Transform ProjectileSpawn { get { return _projectileSpawn; } }

	public override void OnStartAuthority()
	{
        enabled = true;

        PlayerInputHandler.OnLMB += SetLMBState;
	}

    [Client]
    private void SetLMBState(bool lmbState)
    {
        if(lmbState) CmdRequestProjectileSpawn();
        else CmdRequestProjectileFire();
    }

    [Command]
    private void CmdRequestProjectileSpawn() => OnProjectileSpawnRequested?.Invoke(this);

    [Command]
    private void CmdRequestProjectileFire() => OnProjectileFireRequested?.Invoke(this);

}
