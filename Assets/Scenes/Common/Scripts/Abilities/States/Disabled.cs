using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabled : BaseAbilityState
{
    public override BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.DISABLED; } }

    public Disabled(Ability ability, float duration, List<BaseBehaviour> behaviours) : base(ability, duration, behaviours) {}

    public void SetDuration(float duration) => this.duration = duration;
}
