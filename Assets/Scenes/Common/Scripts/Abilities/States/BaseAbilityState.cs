using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityState : IState
{
    protected Ability ability;
    protected List<BaseBehaviour> behaviours = new List<BaseBehaviour>();
    public virtual BaseBehaviour.ExecutionMask BehaviourExecutionMask { get { return BaseBehaviour.ExecutionMask.NONE; } }

    protected Coroutine coroutine = null;

    protected bool stateCompleted = false;
    protected float duration;
    protected float elapsedTime;

    public struct AbilityStateContext
    {
        public AbilityStateContext(IState state, bool stateCompleted, float duration, float elapsedTime)
        {
            this.state = state;
            this.stateCompleted = stateCompleted;
            this.duration = duration;
            this.elapsedTime = elapsedTime;
        }
        public IState state;
        public bool stateCompleted;
        public float duration;
        public float elapsedTime;
    }

    public BaseAbilityState(Ability ability, float duration, List<BaseBehaviour> behaviours)
    {
        this.duration = duration;
        this.ability = ability;

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
            behaviour.Tick(new AbilityStateContext(this, stateCompleted, duration, elapsedTime));
        }
    }

    public void OnEnter()
    {
        stateCompleted = false;
        elapsedTime = 0f;

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnEnter(new AbilityStateContext(this, stateCompleted, duration, elapsedTime));
        }

        Debug.Log("Entering " + this.GetType().ToString() + " (" + duration + "s duration)");
        coroutine = ability.owner.GetComponent<PlayerAbilityManager>().StartCoroutine(RunUntilComplete());
    }

    private IEnumerator RunUntilComplete()
    {
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        
        stateCompleted = true;
        ability.InvokeStateCompleted();
    }

    public void OnExit()
    {
        foreach (BaseBehaviour behaviour in behaviours)
        {
            behaviour.OnExit(new AbilityStateContext(this, stateCompleted, duration, elapsedTime));
        }

        Debug.Log(this.GetType().ToString() + (stateCompleted ? " completed." : " canceled after " + elapsedTime));

        stateCompleted = false;
        
        if (coroutine != null) ability.owner.GetComponent<PlayerAbilityManager>().StopCoroutine(coroutine);
        coroutine = null;
    }
}
