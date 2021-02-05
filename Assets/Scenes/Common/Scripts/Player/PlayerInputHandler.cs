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
    public void CatchMouse(Vector2 ctx) => OnMouse?.Invoke(ctx);

    [Client]
    public void CatchLMB(bool ctx) => OnLMB?.Invoke(ctx);

    [Client]
    public void CatchRMB(bool ctx) => OnRMB?.Invoke(ctx);

    // fuck this, you cant jump for now lol

    // outsource this to a jump input processor
    // Check if grounded in future
    // _forceReceiver.AddForce((Vector3.up + transform.forward.normalized) * 5f);
}
