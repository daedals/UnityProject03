using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabled : BaseAbilityState
{
    public Disabled(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}

    public void SetDuration(float duration) => this.duration = duration;
}
