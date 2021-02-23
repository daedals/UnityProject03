using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : BaseAbilityState
{
    public override BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.CASTING; } }
    public Casting(Ability ability, float duration, List<BaseBehaviour> behaviours) : base(ability, duration, behaviours) {}
}
