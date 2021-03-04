using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBaseMovementSpeed : NetworkBehaviour, IMovementSpeedModifier
{
    [Header("References")]
    [SerializeField] private EntityDataContainer _playerData = null;
    [SerializeField] private MovementSpeedHandler _movementSpeedHandler = null;


	public override void OnStartAuthority()
	{
        enabled = true;
        Value = _playerData.movementSpeed;
	}

    [ClientCallback]
    private void OnEnable() => _movementSpeedHandler.AddModifier(this);
    [ClientCallback]
    private void OnDisable() => _movementSpeedHandler.RemoveModifier(this);

    public float Value { get; private set; }
}
