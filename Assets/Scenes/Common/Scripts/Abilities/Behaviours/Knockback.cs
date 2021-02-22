using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : BaseBehaviour
{
	public Knockback(KnockbackData data) : base((BaseBehaviourData)data) {}

	public override void Tick(BaseAbilityState.AbilityStateContext ctx)
	{
		throw new System.NotImplementedException();
	}

	public override void OnEnter(BaseAbilityState.AbilityStateContext ctx)
	{
		throw new System.NotImplementedException();
	}

	public override void OnExit(BaseAbilityState.AbilityStateContext ctx)
	{
		throw new System.NotImplementedException();
	}

    public override object Clone()
    {
        return null;
    }
}
