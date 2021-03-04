using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementInputProcessor : NetworkBehaviour, IMovementModifier
{
    [Header("References")]
    [SerializeField] private MovementHandler _movementHandler = null;
    [SerializeField] private EntityDataContainer _playerData = null;
    [SerializeField] private MovementSpeedHandler _movementSpeedHandler = null;
    [SerializeField] private CharacterController _controller = null;

    [Header("Debug")]
    [SerializeField] private float _currentSpeed = 0f;

    private Vector3 _previousVelocity = Vector3.zero;
    private Vector2 _previousInputDirection;
    

    [ClientCallback]
    private void OnEnable() => _movementHandler.AddModifier(this);
    [ClientCallback]
    private void OnDisable() => _movementHandler.RemoveModifier(this);

    public Vector3 MMValue { get; private set; }


    [ClientCallback]
    private void Update() => Move();

    public override void OnStartAuthority()
    {
        enabled = true;

        PlayerInputHandler.OnMovement += SetMovementInput;
    }

	[Client]
	public void SetMovementInput(Vector2 inputDirection) => _previousInputDirection = inputDirection;

	[Client]
    private void Move()
    {
        // magnitude is always 1 except without any input, then its 0
        float targetSpeed = _movementSpeedHandler.MovementSpeed * _previousInputDirection.magnitude;

        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, _playerData.acceleration * Time.deltaTime);

        Vector3 movementDirection;

        if (targetSpeed != 0f)
        {
            movementDirection = new Vector3(_previousInputDirection.x, 0f, _previousInputDirection.y);
        }
        else
        {
            movementDirection = _previousVelocity.normalized;
        }

        if(_controller.isGrounded)
        {
            MMValue = movementDirection * _currentSpeed;

        }

        // _previousVelocity = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z);
        _previousVelocity = MMValue;
        // _currentSpeed = _previousVelocity.magnitude;
    }
}
