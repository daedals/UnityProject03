using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ForceReceiver : NetworkBehaviour, IMovementModifier
{
    [Header("References")]
    [SerializeField] private CharacterController _controller = null;
    [SerializeField] private MovementHandler _movementHandler = null;
    [SerializeField] private EntityDataContainer _playerData = null;

    [Header("Debug")]
    [SerializeField] private bool _wasGroundedLastFrame;

	public override void OnStartAuthority()
	{
        enabled = true;
	}

    [ClientCallback]
    private void OnEnable() => _movementHandler.AddModifier(this);

    [ClientCallback]
    private void OnDisable() => _movementHandler.RemoveModifier(this);

    public Vector3 MMValue { get; private set; }

    [Client]
    private void Update() 
    {
        if(!_wasGroundedLastFrame && _controller.isGrounded)
        {
            MMValue = new Vector3(MMValue.x, 0f, MMValue.z);
        }

        _wasGroundedLastFrame = _controller.isGrounded;

        if (MMValue.magnitude < .2f)
        {
            MMValue = Vector3.zero;
        }

        MMValue = Vector3.Lerp(MMValue, Vector3.zero, _playerData.drag * Time.deltaTime);
    }

    [Client]
    // public void AddForce(Vector3 force) => MMValue += force / _playerData.mass;
    
    public void AddForce(Vector3 force)
    {
        Debug.Log($"Added force vector {force} to {gameObject.name} with mass of {_playerData.mass}.");
        MMValue += force / _playerData.mass;
    }
}
