using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityState : IState
{
    protected float duration;
    protected AbilityStateMachine stateMachine;
    protected List<BaseBehaviour> behaviours = new List<BaseBehaviour>();
    public virtual BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.NONE; } }

    protected Coroutine coroutine = null;


    public BaseAbilityState(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours)
    {
        this.duration = duration;
        this.stateMachine = stateMachine;

        foreach(BaseBehaviour behaviour in behaviours)
        {
            Debug.Log(this.GetType().ToString() + " " + behaviour.GetType().ToString() + " " + BehaviourExecutionMask + " " + behaviour.Data.ExecutionMask + " " + ((behaviour.Data.ExecutionMask & BehaviourExecutionMask) != 0));
        }

        // each ability state has an execution flag to indicate which state it represents. Each behaviour has a mask to decide 
        // when to become active.

        this.behaviours = behaviours.FindAll(
            delegate(BaseBehaviour behaviour) 
            { 
                return (behaviour.Data.ExecutionMask & BehaviourExecutionMask) != 0; 
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
            Debug.Log(behaviour.GetType().ToString() + " is active in this State.");
            behaviour.OnEnter();
        }

        Debug.Log("Entering State " + this.GetType().ToString() + " (" + duration + " s duration)");
        coroutine = stateMachine.owner.GetComponent<PlayerAbilityManager>().StartCoroutine(RunUntilComplete());
    }

    private IEnumerator RunUntilComplete()
    {
        yield return new WaitForSeconds(duration);
        
        stateMachine.InvokeStateCompleted();
    }

    public void OnExit()
    {
        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnExit();
        }

        coroutine = null;
    }
}
