using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotateTowardsMouse : BaseBehaviour, IRotationModifier
{
    public override BaseBehaviourData Data { get => data; set => data = (RotateTowardsMouseData)value; }
	[SerializeField] protected new RotateTowardsMouseData data;


    public Quaternion RMValue { get; private set; }
    public int RMPriority { get; private set; }

    private RotationHandler rotationHandler 
    {
        get => transform.parent.GetComponent<RotationHandler>();
    }

	public override void Initialize()
	{
        RMPriority = 2;
	}

	public override void OnEnter(BaseAbilityState.AbilityStateContext ctx)
	{
        if (hasAuthority)
        {
            SetRMValue();
            rotationHandler.AddModifier(this);
        }
	}

	public override void Tick(BaseAbilityState.AbilityStateContext ctx) => SetRMValue();

    private void SetRMValue()
    {
        Vector3 relativeMousePosition = Vector3.ProjectOnPlane(PlayerInputHandler.GetMousePositionWorldSpace() - transform.position, Vector3.up);
        RMValue = Quaternion.Euler(0f, Vector3.SignedAngle(Vector3.forward, relativeMousePosition, Vector3.up), 0f);
    }

	public override void OnExit(BaseAbilityState.AbilityStateContext ctx)
	{
        if (hasAuthority)
        {
            rotationHandler.RemoveModifier(this);
        }
	}
}
