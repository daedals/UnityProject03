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
		if (obj.GetComponent<MecanimAbility>() == null) throw new System.Exception("Could not assign behaviour to GameObject without ability script.");

		string s = this.GetType().ToString();
		string behaviourName = s.Substring(0, s.Length - 4);

		var behaviour = obj.GetComponent(System.Type.GetType(behaviourName)) as BaseBehaviour;
		behaviour.Data = this;

		behaviour.enabled = true;
	}
}


[RequireComponent(typeof(MecanimAbility))]
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
        ALL = (1 << 6) - 1
	}

	// the following to types are a hacky method to use type covariance in c# 
	// see: https://stackoverflow.com/questions/421851/how-to-return-subtype-in-overridden-method-of-subclass-in-c

	public virtual BaseBehaviourData Data { get; set;}

	private void OnEnable() {}

	public virtual void Initialize() {}

    public virtual void Tick(BaseAbilityState.AbilityStateContext ctx) {}
    public virtual void OnEnter(BaseAbilityState.AbilityStateContext ctx) {}
    public virtual void OnExit(BaseAbilityState.AbilityStateContext ctx) {}

}

public static class BaseBehaviourExtension
{
	public static List<BaseBehaviour> SelectRelevant(this List<BaseBehaviour> behaviours, BaseBehaviour.ExecutionMask executionMask)
	{
        List<BaseBehaviour> relevant = behaviours.FindAll(
            delegate(BaseBehaviour behaviour) 
            { 
                return (behaviour.Data.ExecutionMask & executionMask) != 0; 
            });

		return relevant;
	}
}


[System.Serializable]
public class ExecutionMaskDurationEntry
{
	public ExecutionMaskDurationEntry(BaseBehaviour.ExecutionMask executionMask, float duration)
	{
		this.executionMask = executionMask;
		this.duration = duration;
	}

	public BaseBehaviour.ExecutionMask executionMask;
	public float duration;
}

public static class ExecutionMaskDurationEntryExtension
{
	public static float Get(this List<ExecutionMaskDurationEntry> entries, BaseBehaviour.ExecutionMask mask)
	{
		ExecutionMaskDurationEntry entry = entries.Find(entry => entry.executionMask == mask);

		if (entry == null) return 0f;

		return entry.duration;
	}
}
