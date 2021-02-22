using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityState : IState
{
    protected AbilityStateMachine stateMachine;
    protected List<BaseBehaviour> behaviours = new List<BaseBehaviour>();
    public virtual BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.NONE; } }

    protected Coroutine coroutine = null;

    protected bool stateCompleted = false;
    protected float duration;
    protected float elapsedTime;

    public struct AbilityStateContext
    {
        public AbilityStateContext(bool stateCompleted, float duration, float elapsedTime)
        {
            this.stateCompleted = stateCompleted;
            this.duration = duration;
            this.elapsedTime = elapsedTime;
        }
        public bool stateCompleted;
        public float duration;
        public float elapsedTime;
    }

    public BaseAbilityState(AbilityStateMachine stateMachine, float duration, List<BaseBehaviour> behaviours)
    {
        this.duration = duration;
        this.stateMachine = stateMachine;

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
            behaviour.Tick(new AbilityStateContext(stateCompleted, duration, elapsedTime));
        }
    }

    public void OnEnter()
    {
        stateCompleted = false;
        elapsedTime = 0f;

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnEnter(new AbilityStateContext(stateCompleted, duration, elapsedTime));
        }

        Debug.Log("OnEnter " + this.GetType().ToString() + " (" + duration + " s duration)");
        coroutine = stateMachine.owner.GetComponent<PlayerAbilityManager>().StartCoroutine(RunUntilComplete());
    }

    private IEnumerator RunUntilComplete()
    {
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        // yield return new WaitForSeconds(duration);

        Debug.Log("State completed.");
        stateCompleted = true;
        stateMachine.InvokeStateCompleted();
    }

    public void OnExit()
    {
        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnExit(new AbilityStateContext(stateCompleted, duration, elapsedTime));
        }

        stateCompleted = false;
        Debug.Log("OnExit " + this.GetType().ToString());
        coroutine = null;
    }
}
