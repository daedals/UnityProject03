using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void SubscribtableAction();

public class StateMachine
{
    public IState currentState;
    public IState nextState;

    private Dictionary<System.Type, List<Transition>> _transitions = new Dictionary<System.Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(capacity: 0);

    public void Tick()
    {
        if (nextState != null) SetState();
        currentState?.Tick();
    }

    protected void InitiateTransition(IState state)
    {
        nextState = state;
    }

    protected void SetState()
    {
        if (nextState == currentState) return;

        currentState?.OnExit();

        foreach (Transition transition in _currentTransitions)
        {
            transition.Unsubscribe();
        }

        currentState = nextState;
        nextState = null;
        
        // Debug.Log("Subscribing to state " + currentState.GetType().ToString() + "s possible transitions");

        _transitions.TryGetValue(currentState.GetType(), out _currentTransitions);

        if (_currentTransitions == null)
        {
            Debug.Log("Transitions are empty.");
            _currentTransitions = EmptyTransitions;
        }
        
        foreach (Transition transition in _currentTransitions)
        {
            transition.Subscribe();
        }

        currentState.OnEnter();
    }

    private class Transition
    {
        private IState to { get; }
        private Action<IState> SetState;
        private bool active = false;
        
        public Transition(IState to, ref SubscribtableAction trigger, Action<IState> SetState)
        {
            this.to = to;
            trigger += Trigger;
            this.SetState = SetState;
        }
        private void Trigger()
        {
            if (active) SetState(to);
        }
        public void Subscribe() => active = true;
        public void Unsubscribe() => active = false;
    }

    public void Start(IState state)
    {
		InitiateTransition(state);
		SetState();
    }

    public void AddTransition(IState from, IState to, ref SubscribtableAction trigger)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, ref trigger, InitiateTransition));
    }

    public void AddAnyTransition(IState to, ref SubscribtableAction trigger)
    {
        Transition transition = new Transition(to, ref trigger, InitiateTransition);
        _anyTransitions.Add(transition);
        transition.Subscribe();
    }
}
