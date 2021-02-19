using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupted : BaseAbilityState
{
    public override BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.INTERRUPTED; } }

    public Interrupted(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}
}
