using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : BaseAbilityState
{
    public Casting(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}
}
