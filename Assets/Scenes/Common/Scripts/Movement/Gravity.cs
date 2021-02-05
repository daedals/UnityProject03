using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Gravity : NetworkBehaviour, IMovementModifier
{
    [Header("References")]
    [SerializeField] private CharacterController _controller = null;
    [SerializeField] private MovementHandler _movementHandler = null;

    [Header("Settings")]
    [SerializeField] private float _groundedPullMagnitude = 5f;
    [SerializeField] private float _gravityMagnitude = Physics.gravity.y;

    private bool _wasGroundedLastFrame;

	public override void OnStartAuthority()
	{
        enabled = true;
	}
	

    [ClientCallback]
    private void OnEnable() => _movementHandler.AddModifier(this);
    [ClientCallback]
    private void OnDisable() => _movementHandler.RemoveModifier(this);

    public Vector3 MMValue { get; private set; }
    
    [ClientCallback]
    private void Update() => ProcessGravity();

    [Client]
    private void ProcessGravity()
    {
        if (_controller.isGrounded)
        {
            MMValue = new Vector3(MMValue.x, -_groundedPullMagnitude, MMValue.z);
        }
        else if (_wasGroundedLastFrame)
        {
            MMValue = Vector3.zero;
        }
        else
        {
            MMValue = new Vector3(MMValue.x, MMValue.y + _gravityMagnitude * Time.deltaTime, MMValue.z);
        }

        _wasGroundedLastFrame = _controller.isGrounded;
    }
}
