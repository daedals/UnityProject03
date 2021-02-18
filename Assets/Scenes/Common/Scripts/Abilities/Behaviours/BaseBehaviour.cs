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
		get 
		{
			return executionMask;
		}
		protected set
		{
			executionMask = value;
		}
	}
}


public abstract class BaseBehaviour : System.ICloneable
{
    [System.Flags]
	public enum ExecutionMask
	{
        NONE = 0,
		INACTIVE = 1 << 1,
		CHANNELING = 1 << 2,
		CASTING = 1 << 3,
		ONCOOLDOWN = 1 << 4,
		DISABLED = 1 << 5,
		INTERRUPTED = 1 << 6,
        ALL = 1 << 7 - 1
	}
	public BaseBehaviourData data;

	public BaseBehaviour(BaseBehaviourData data)
	{
		this.data = data;
	}

    public abstract void Tick();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract object Clone();
}
