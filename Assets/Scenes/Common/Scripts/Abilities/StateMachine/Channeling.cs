using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channeling : IState
{
    private readonly AbilityStateMachine stateMachine;
    private readonly float duration;

    private Coroutine channel = null;

    public Channeling(AbilityStateMachine stateMachine, float duration)
    {
        this.stateMachine = stateMachine;
        this.duration = duration;
    }

    public void Tick()
    {

    }

    public void OnEnter()
    {
        channel = stateMachine.StartCoroutine(Channel());
    }

    private IEnumerator Channel()
    {
        stateMachine.isChanneling = true;
        yield return new WaitForSeconds(duration);
        stateMachine.isChanneling = false;
    }

    public void OnExit()
    {
        channel = null;
    }
}
