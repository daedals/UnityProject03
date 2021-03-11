using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public abstract class BaseBehaviourData : ScriptableObject
{
	private bool executionMaskChangeable = false;

    private BaseBehaviour.ExecutionMask executionMask;

	public BaseBehaviour.ExecutionMask ExecutionMask 
	{
		get => executionMask;
		protected set => executionMask = value;
	}

	public void Setup(GameObject obj)
	{
		if (obj.GetComponent<Ability>() == null) throw new System.Exception("Could not assign behaviour to GameObject wihtout ability script.");

		string s = this.GetType().ToString();
		string behaviourName = s.Substring(0, s.Length - 4);

		var behaviour = obj.GetComponent(System.Type.GetType(behaviourName)) as BaseBehaviour;
		behaviour.Data = this;

		behaviour.enabled = true;
	}
}


[RequireComponent(typeof(Ability))]
public abstract class BaseBehaviour : NetworkBehaviour
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

	// the following to types are a hacky method to use type covariance in c# 
	// see: https://stackoverflow.com/questions/421851/how-to-return-subtype-in-overridden-method-of-subclass-in-c

	public virtual BaseBehaviourData Data { get; set;}

	public virtual void Initialize() {}

    public virtual void Tick(BaseAbilityState.AbilityStateContext ctx) {}
    public virtual void OnEnter(BaseAbilityState.AbilityStateContext ctx) {}
    public virtual void OnExit(BaseAbilityState.AbilityStateContext ctx) {}

}
