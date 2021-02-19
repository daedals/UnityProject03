using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : BaseBehaviour
{
	public Knockback(KnockbackData data) : base((BaseBehaviourData)data) {}

	public override void Tick()
	{
		throw new System.NotImplementedException();
	}

	public override void OnEnter()
	{
		throw new System.NotImplementedException();
	}

	public override void OnExit()
	{
		throw new System.NotImplementedException();
	}

    public override object Clone()
    {
        return null;
    }
}
