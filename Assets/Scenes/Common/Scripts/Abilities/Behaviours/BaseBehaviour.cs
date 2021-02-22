using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseBehaviourData : ScriptableObject
{
	public System.Type GetBehaviourType()
	{
		string s = this.GetType().ToString();
		return System.Type.GetType(s.Substring(0, s.Length - 4));
	}

	private bool executionMaskChangeable = false;

    private BaseBehaviour.ExecutionMask executionMask;

	public BaseBehaviour.ExecutionMask ExecutionMask 
	{
		get => executionMask;
		protected set => executionMask = value;
	}
}


public abstract class BaseBehaviour : System.ICloneable
{
    [System.Flags]
	public enum ExecutionMask
	{
        NONE = 0,
		INACTIVE = 1 << 0,
		CHANNELING = 1 << 1,
		CASTING = 1 << 2,
		ONCOOLDOWN = 1 << 3,
		DISABLED = 1 << 4,
		INTERRUPTED = 1 << 5,
        ALL = 1 << 6 - 1
	}
	public BaseBehaviourData Data { get; protected set; }
    protected AbilityStateMachine stateMachine = null;

	public BaseBehaviour(BaseBehaviourData data)
	{
		this.Data = data;
	}

    public void Initialize(AbilityStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Tick(BaseAbilityState.AbilityStateContext ctx);
    public abstract void OnEnter(BaseAbilityState.AbilityStateContext ctx);
    public abstract void OnExit(BaseAbilityState.AbilityStateContext ctx);
    public abstract object Clone();
}
