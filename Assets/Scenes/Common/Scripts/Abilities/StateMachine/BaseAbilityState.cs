using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityState : IState
{
    protected float duration;
    protected AbilityStateMachine stateMachine;
    protected List<BaseBehaviour> behaviours = new List<BaseBehaviour>();
    protected BaseBehaviour.ExecutionMask behaviourExecutionMask = BaseBehaviour.ExecutionMask.NONE;

    protected Coroutine coroutine = null;


    public BaseAbilityState(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours)
    {
        this.duration = duration;
        this.stateMachine = stateMachine;

        this.behaviours = behaviours.FindAll(
            delegate(BaseBehaviour behaviour) 
            { 
                return (behaviour.executionMask & behaviourExecutionMask) != 0; 
            });
    }

    public void Tick() 
    {
        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.Tick();
        }
    }

    public void OnEnter()
    {
        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnEnter();
        }

        Debug.Log(this.GetType().ToString() + " OnEnter method called.");
        coroutine = stateMachine.owner.GetComponent<PlayerAbilityManager>().StartCoroutine(RunUntilComplete());
    }

    private IEnumerator RunUntilComplete()
    {
        Debug.Log(this.GetType().ToString() + " coroutine started.");
        yield return new WaitForSeconds(duration);
        Debug.Log(this.GetType().ToString() + " state change event invoked.");
        stateMachine.InvokeStateCompleted();
    }

    public void OnExit()
    {
        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnExit();
        }

        Debug.Log(this.GetType().ToString() + " OnExit method called.");
        coroutine = null;
    }
}
