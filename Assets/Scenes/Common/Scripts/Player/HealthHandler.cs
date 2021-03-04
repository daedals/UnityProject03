using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class HealthHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private EntityDataContainer _playerData = null;

    [SyncVar] private float _currentHealth;

    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        private set
        {
            _currentHealth = value;
            _playerData.CurrentHealth = value;
        }
    }

    public delegate void PlayerDeathDelegate(uint netID);
    public event PlayerDeathDelegate PlayerDeath;

    public delegate void HealthChangedDelegate(float currentHealth, float maximumHealth);
    public event HealthChangedDelegate EventHealthChanged;

    #region Server

    [Server]
    private void SetHealth(float value)
    {
        CurrentHealth = value;
        EventHealthChanged?.Invoke(CurrentHealth, _playerData.MaximumHealth);

        if (CurrentHealth <= 0f)
        {
            PlayerDeath?.Invoke(GetComponent<NetworkIdentity>().netId);
        }
    }

	public override void OnStartServer() => SetHealth(_playerData.MaximumHealth);

	[Command]
	public void CmdDealDamage(float damage) => SetHealth(Mathf.Max(CurrentHealth - damage, 0f));

	#endregion
}
