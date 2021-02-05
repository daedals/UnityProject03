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
    [SerializeField] private float _magnitude = 0f;
    [SerializeField] private bool _active = false;
    [SerializeField] private bool _isGrounded = false;

    private Vector3 _previousInputDirection;
    private Quaternion _previousRotation;


    [ClientCallback]
    private void OnEnable()
    {
        RMPriority = 0;
        _previousRotation = transform.rotation;
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

        // change this subscription, rotation should only happen 
        PlayerInputHandler.OnMovement += SetMovementInput;
	}

	

    [Client]
    public void SetMovementInput(Vector2 inputDirection)
    {
        _previousInputDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);

        _magnitude = _previousInputDirection.magnitude;
        _isGrounded = _controller.isGrounded;

        if(_active && (_previousInputDirection.magnitude < .2f || !_controller.isGrounded))
        {
            _active = false;
            _rotationHandler.RemoveModifier(this);
        }
        else
        {
            _active = true;
            _rotationHandler.AddModifier(this);
        }
    }

    [ClientCallback]
    private void Update() => Rotate();

    [Client]
    private void Rotate()
    {
        if (_active)
        {
            Quaternion targetRotation;
            targetRotation = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, _previousInputDirection, Vector3.up), 0f);

            RMValue = targetRotation;
        }
    }
}
