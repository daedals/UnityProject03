using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotationInputProcessor : NetworkBehaviour, IRotationModifier
{
    [Header("References")]
    [SerializeField] private RotationHandler _rotationHandler = null;
    [SerializeField] private LayerMask _clickMask;
    

    private Vector2 _mousePosition;
    private Quaternion _previousRotation;
    private bool _lmbState = false;



    [ClientCallback]
    private void OnEnable() => RMPriority = 2;


    [ClientCallback]
    private void OnDisable()
    {
        // remove from _rotationHandler list if its in it
    }

    public Quaternion RMValue { get; private set; }
    public int RMPriority { get; private set; }


    [ClientCallback]
    private void Update() => Rotate();

	public override void OnStartAuthority()
	{
        enabled = true;
        
        PlayerInputHandler.OnMouse += SetMousePosition;
        PlayerInputHandler.OnLMB += SetLMBState;
	}

    
    [Client]
    public void SetMousePosition(Vector2 mousePosition)
    {
        _mousePosition = mousePosition;
    }

    [Client]
    public void SetLMBState(bool lmbState)
    {
        _lmbState = lmbState;
        if (_lmbState)
        {
            _rotationHandler.AddModifier(this);
        }
        else
        {
            _rotationHandler.RemoveModifier(this);
        }
    }

    [Client]
    private void Rotate()
    {
        if (_lmbState)
        {
            Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
            RaycastHit hit;

            Vector3 relativeMousePosition;

            if (Physics.Raycast(ray, out hit, 100f, _clickMask))
            {
                relativeMousePosition = Vector3.ProjectOnPlane(hit.point - transform.position, Vector3.up);
            }
            else
            {
                relativeMousePosition = Vector3.forward;
            }

            RMValue = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, relativeMousePosition, Vector3.up), 0f);
        }
    }
}
