using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine
{
    public IState currentState;

    private Dictionary<System.Type, List<Transition>> _transitions = new Dictionary<System.Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(capacity: 0);

    private Coroutine update = null;

    public StateMachine(MonoBehaviour coroutineSlave)
    {
        if (coroutineSlave != null)
        {
            update = coroutineSlave.GetComponent<PlayerAbilityManager>().StartCoroutine(Update());
        }
    }

    protected IEnumerator Update()
    {
        while (true)
        {
            currentState?.Tick();
            yield return 0;
        }
    }

    protected void SetState(IState state)
    {
        if (state == currentState) return;

        currentState?.OnExit();

        foreach (Transition transition in _currentTransitions)
        {
            transition.Unsubscribe();
        }

        currentState = state;

        _transitions.TryGetValue(currentState.GetType(), out _currentTransitions);

        if (_currentTransitions == null)
        {
            _currentTransitions = EmptyTransitions;
        }
        
        foreach (Transition transition in _currentTransitions)
        {
            transition.Subscribe();
        }

        currentState.OnEnter();
    }

    protected class Transition
    {
        private IState to { get; }
        private event Action trigger;
        private event Action<IState> internalTrigger;
        
        public Transition(IState to, Action trigger, Action<IState> setState)
        {
            this.to = to;
            this.trigger = trigger;

            internalTrigger += setState;
        }

        private void Trigger() => internalTrigger.Invoke(to);

        public void Subscribe() => trigger += Trigger;

        public void Unsubscribe() => trigger -= Trigger;
    }

    protected void AddTransition(IState from, IState to, Action trigger)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, trigger, SetState));
    }

    protected void AddAnyTransition(IState to, Action trigger)
    {
        Transition transition = new Transition(to, trigger, SetState);
        _anyTransitions.Add(transition);
        transition.Subscribe();
    }
}
