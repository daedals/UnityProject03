using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityState : IState
{
	public void OnEnter()
	{
		throw new System.NotImplementedException();
	}

	public void OnExit()
	{
		throw new System.NotImplementedException();
	}

	public void Tick()
	{
		throw new System.NotImplementedException();
	}

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

}
