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
        var transition = GetTransition();

        if (transition != null)
        {
            SetState(transition.To);
        }

        _currentState?.Tick();
    }

    protected void SetState(IState state)
    {
        if (state == _currentState) return;

        _currentState?.OnExit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);

        if (_currentTransitions == null)
        {
            _currentTransitions = EmptyTransitions;
        }

        _currentState.OnEnter();
    }

    protected class Transition
    {
        public IState To { get; }
        public Func<bool> Condition { get; }
        
        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    protected void AddTransition(IState from, IState to, Func<bool> predecate)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predecate));
    }

    protected void AddAnyTransition(IState to,Func<bool> predecate)
    {
        _anyTransitions.Add(new Transition(to, predecate));
    }

    protected Transition GetTransition()
    {
        foreach (Transition transition in _anyTransitions)
        {
            if (transition.Condition())
                return transition;
        }
        
        foreach (Transition transition in _currentTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        return null;
    }
}
