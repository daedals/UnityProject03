using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channeling : BaseAbilityState
{
    public override BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.CHANNELING; } }

    public Channeling(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}
}
