using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCooldown : BaseAbilityState
{
    public OnCooldown(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}
}
