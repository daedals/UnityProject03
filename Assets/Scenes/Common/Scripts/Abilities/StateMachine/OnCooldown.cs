using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCooldown : IState
{
    private readonly AbilityStateMachine stateMachine;
    private readonly float duration;

    private Coroutine cooldown = null;

    public OnCooldown(AbilityStateMachine stateMachine, float duration)
    {
        this.stateMachine = stateMachine;
        this.duration = duration;
    }

    public void Tick()
    {

    }

    public void OnEnter()
    {
        cooldown = stateMachine.StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        stateMachine.isOnCooldown = true;
        yield return new WaitForSeconds(duration);
        stateMachine.isOnCooldown = false;
    }

    public void OnExit()
    {
        cooldown = null;
    }
}
