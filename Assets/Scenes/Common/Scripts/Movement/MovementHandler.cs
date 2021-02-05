using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _controller = null;

    private readonly List<IMovementModifier> _modifiers = new List<IMovementModifier>();

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void Update() => Move();

    [Client]
    public void AddModifier(IMovementModifier modifier) => _modifiers.Add(modifier);
    
    [Client]
    public void RemoveModifier(IMovementModifier modifier) => _modifiers.Remove(modifier);

    [Client]
    private void Move()
    {
        Vector3 movement = Vector3.zero;

        foreach (IMovementModifier modifier in _modifiers)
        {
            movement += modifier.MMValue;
        }

        _controller.Move(movement * Time.deltaTime);
    }
}
