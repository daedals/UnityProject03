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

	public GameObject CreateBehaviourObject()
	{
		string s = this.GetType().ToString();
		string behaviourName = s.Substring(0, s.Length - 4);

		GameObject behaviourObject = new GameObject(behaviourName);

		var behaviour = behaviourObject.AddComponent(System.Type.GetType(behaviourName)) as BaseBehaviour;
		behaviour.Data = this;

		return behaviourObject;
	}
}


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

	public virtual BaseBehaviourData Data { get => data; set => data = value; }
	protected BaseBehaviourData data;

    protected Ability ability = null;

    public virtual void Initialize(Ability ability)
    {
        this.ability = ability;
    }

    public virtual void Tick(BaseAbilityState.AbilityStateContext ctx) {}
    public virtual void OnEnter(BaseAbilityState.AbilityStateContext ctx) {}
    public virtual void OnExit(BaseAbilityState.AbilityStateContext ctx) {}

}
