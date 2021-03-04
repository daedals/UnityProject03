using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _controller = null;
    [Header("Debug")]
    [SerializeField] private float _movementSpeed;

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

        _movementSpeed = (new Vector2(movement.x, movement.z)).magnitude;
        _controller.Move(movement * Time.deltaTime);
    }
}
