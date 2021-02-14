using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channeling : BaseAbilityState
{
    public Channeling(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours) : base(stateMachine, duration, behaviours) {}
}
