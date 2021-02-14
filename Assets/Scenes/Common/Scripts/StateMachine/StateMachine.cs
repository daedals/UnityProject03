using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine : MonoBehaviour
{
    private IState _currentState;

    private Dictionary<System.Type, List<Transition>> _transitions = new Dictionary<System.Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(capacity: 0);


    protected void Update()
    {
        // var transition = GetTransition();

        // if (transition != null)
        // {
        //     SetState(transition.To);
        // }

        _currentState?.Tick();
    }

    protected void SetState(IState state)
    {
        if (state == _currentState) return;

        _currentState?.OnExit();

        foreach (Transition transition in _currentTransitions)
        {
            transition.Unsubscribe();
        }

        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);

        if (_currentTransitions == null)
        {
            _currentTransitions = EmptyTransitions;
        }
        
        foreach (Transition transition in _currentTransitions)
        {
            transition.Subscribe();
        }

        _currentState.OnEnter();
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

    // protected Transition GetTransition()
    // {
    //     foreach (Transition transition in _anyTransitions)
    //     {
    //         if (transition.Condition())
    //             return transition;
    //     }
        
    //     foreach (Transition transition in _currentTransitions)
    //     {
    //         if (transition.Condition())
    //             return transition;
    //     }

    //     return null;
    // }
}
