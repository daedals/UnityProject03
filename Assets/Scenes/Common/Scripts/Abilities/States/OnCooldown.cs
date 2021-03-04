using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCooldown : BaseAbilityState
{
    public override BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.ONCOOLDOWN; } }

    public OnCooldown(Ability ability, float duration, List<BaseBehaviour> behaviours) : base(ability, duration, behaviours) {}
}