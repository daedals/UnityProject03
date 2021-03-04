using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerInputHandler : NetworkBehaviour
{
    public static event Action<Vector2> OnMovement;
    public static event Action<bool> OnLMB;
    public static event Action<bool> OnRMB;
    public static event Action<Vector2> OnMouse;

    private static Vector2 mousePosition = Vector2.zero; 

	public override void OnStartAuthority()
	{
        enabled = true;

        InputHandler.Controls.Player.Movement.performed += ctx => CatchMovement(ctx.ReadValue<Vector2>());
        InputHandler.Controls.Player.Movement.canceled += ctx => CatchMovement(ctx.ReadValue<Vector2>());

        InputHandler.Controls.Player.Mouse.performed += ctx => CatchMouse(ctx.ReadValue<Vector2>());
        InputHandler.Controls.Player.Mouse.canceled += ctx => CatchMouse(ctx.ReadValue<Vector2>());
        
        InputHandler.Controls.Player.LMB.performed += ctx => CatchLMB(ctx.ReadValue<float>() > .5f);
        InputHandler.Controls.Player.LMB.canceled += ctx => CatchLMB(ctx.ReadValue<float>() > .5f);
        
        InputHandler.Controls.Player.RMB.performed += ctx => CatchRMB(ctx.ReadValue<float>() > .5f);
        InputHandler.Controls.Player.RMB.canceled += ctx => CatchRMB(ctx.ReadValue<float>() > .5f);
	}

	[Client]
	private void CatchMovement(Vector2 ctx) => OnMovement?.Invoke(ctx);

    [Client]
    public void CatchMouse(Vector2 ctx)
    {
        mousePosition = ctx;
        OnMouse?.Invoke(ctx);
    }

    [Client]
    public static Vector3 GetMousePositionWorldSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        Vector3 mousePositionWorldSpace;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.NameToLayer("MousePositionLayer")))
        {
            mousePositionWorldSpace = Vector3.ProjectOnPlane(hit.point, Vector3.up);
        }
        else
        {
            Debug.Log("Raycast did not hit layer mask.");
            mousePositionWorldSpace = Vector2.zero;
        }

        return mousePositionWorldSpace;
    }

	[Client]
	public static Vector2 GetMousePosition() => mousePosition;

	[Client]
    public void CatchLMB(bool ctx) => OnLMB?.Invoke(ctx);

    [Client]
    public void CatchRMB(bool ctx) => OnRMB?.Invoke(ctx);

    // fuck this, you cant jump for now lol

    // outsource this to a jump input processor
    // Check if grounded in future
    // _forceReceiver.AddForce((Vector3.up + transform.forward.normalized) * 5f);
}
