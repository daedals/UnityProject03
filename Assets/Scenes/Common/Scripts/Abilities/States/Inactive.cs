using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inactive : IState
{
    public BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.INACTIVE; } }

    public void Tick() {}

    public void OnEnter() {}

    public void OnExit() {}
}
