using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementSpeedHandler : NetworkBehaviour
{
    [SerializeField] private float _debugMovementSpeed;
    public float MovementSpeed { get; private set; }
    private readonly List<IMovementSpeedModifier> _modifiers = new List<IMovementSpeedModifier>();

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void Update() => SetMovementSpeed();

    [Client]
    public void AddModifier(IMovementSpeedModifier modifier) => _modifiers.Add(modifier);
    
    [Client]
    public void RemoveModifier(IMovementSpeedModifier modifier) => _modifiers.Remove(modifier);

    [Client]
    private void SetMovementSpeed()
    {
        float movementSpeed = 1;

        foreach (IMovementSpeedModifier modifier in _modifiers)
        {
            movementSpeed *= modifier.Value;
        }

        _debugMovementSpeed = movementSpeed;
        MovementSpeed = movementSpeed;
    }

}
