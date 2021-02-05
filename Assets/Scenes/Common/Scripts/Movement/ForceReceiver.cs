using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ForceReceiver : NetworkBehaviour, IMovementModifier
{
    [Header("References")]
    [SerializeField] private CharacterController _controller = null;
    [SerializeField] private MovementHandler _movementHandler = null;

    [Header("Settings")]
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _drag = 5f;

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

    [ClientCallback]
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

        MMValue = Vector3.Lerp(MMValue, Vector3.zero, _drag * Time.deltaTime);
    }

    [Client]
    public void AddForce(Vector3 force) => MMValue += force / _mass;
}
