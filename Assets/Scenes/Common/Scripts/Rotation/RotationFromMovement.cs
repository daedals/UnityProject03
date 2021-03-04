using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotationFromMovement : NetworkBehaviour, IRotationModifier
{
    
    [Header("References")]
    [SerializeField] private CharacterController _controller = null;
    [SerializeField] private RotationHandler _rotationHandler = null;

    [Header("Debug")]
    [SerializeField] private bool _active = false;
    [SerializeField] private bool _isGrounded = false;


    [ClientCallback]
    private void OnEnable()
    {
        RMPriority = 0;
    }

    [ClientCallback]
    private void OnDisable()
    {
        _rotationHandler.RemoveModifier(this);
    } 

    public Quaternion RMValue { get; private set; }
    public int RMPriority { get; private set; }

	public override void OnStartAuthority()
	{
        enabled = true;
	}

    [ClientCallback]
    private void Update() => Rotate();

    [Client]
    private void Rotate()
    {
        Vector3 movementDirection = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z);
        _isGrounded = _controller.isGrounded;

        if (_active && (movementDirection.magnitude < .2f || !_isGrounded))
        {
            _active = false;
            _rotationHandler.RemoveModifier(this);
        }
        if (!_active && _isGrounded && movementDirection.magnitude >= .2f)
        {
            _active = true;
            _rotationHandler.AddModifier(this);
        }

        if (_active)
        {
            RMValue = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, movementDirection, Vector3.up), 0f);
        }
    }
}
