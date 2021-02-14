using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : IState
{
    private readonly AbilityStateMachine stateMachine;
    private readonly float duration;

    private Coroutine cast = null;

    public Casting(AbilityStateMachine stateMachine, float duration)
    {
        this.stateMachine = stateMachine;
        this.duration = duration;
    }

    public void Tick() {}

    public void OnEnter()
    {
        cast = stateMachine.StartCoroutine(Cast());
    }

    private IEnumerator Cast()
    {
        stateMachine.isCasting = true;
        yield return new WaitForSeconds(duration);
        stateMachine.isCasting = false;
    }

    public void OnExit()
    {
        cast = null;
    }
}
