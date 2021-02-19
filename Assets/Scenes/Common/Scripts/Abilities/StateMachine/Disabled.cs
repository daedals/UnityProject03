using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabled : BaseAbilityState
{
    public override BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.DISABLED; } }

    public Disabled(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}

    public void SetDuration(float duration) => this.duration = duration;
}
