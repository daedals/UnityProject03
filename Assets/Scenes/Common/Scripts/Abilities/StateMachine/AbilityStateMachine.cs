using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityStateMachine : StateMachine
{
	public float channelDuration;
	public float castDuration;
	public float cooldownDuration;
	public float interruptionDuration;
	public float disableDuration;

	private readonly List<BaseBehaviour> behaviours = new List<BaseBehaviour>();

	public GameObject owner;
	public AbilityTemplate template;

	
	public AbilityStateMachine(List<BaseBehaviour> behaviours, AbilityTemplate template, GameObject owner) : base(owner?.GetComponent<PlayerAbilityManager>())
	{
		this.behaviours = behaviours;
		this.owner = owner;
		this.template = template;
	}

	public void Initialize()
	{
		/* TODO: properly setup data */
		channelDuration = template.channelDuration;
		castDuration = template.castDuration;
		cooldownDuration = template.cooldownDuration;
		interruptionDuration = template.interruptionDuration;
		disableDuration = 0f;
		
		// Setup all relevant states
		var inactive = new Inactive();
		var channeling = new Channeling(this, channelDuration, behaviours);
		var casting = new Casting(this, castDuration, behaviours);
		var onCooldown = new OnCooldown(this, cooldownDuration, behaviours);
		var disabled = new Disabled(this, disableDuration, behaviours);
		var interrupted = new Interrupted(this, interruptionDuration, behaviours);

		// connect states with transitions
		AddTransition(inactive, channeling, ref AbilityTriggerPressed);

		AddTransition(channeling, inactive, ref AbilityTriggerReleased);
		AddTransition(channeling, interrupted, ref AbilityInterrupted);
		AddTransition(channeling, casting, ref StateCompleted);

		AddTransition(casting, interrupted, ref AbilityInterrupted);
		AddTransition(casting, onCooldown, ref StateCompleted);

		AddTransition(onCooldown, inactive, ref StateCompleted);

		AddTransition(interrupted, inactive, ref StateCompleted);
		
		AddTransition(disabled, inactive, ref AbilityEnabled);
		AddAnyTransition(disabled, ref AbilityDisabled);

		// set starting position
		InitiateTransition(inactive);
		SetState();
		
		Debug.Log("Initialized State machine to state " + currentState.GetType().ToString());
	}

	#region Events
	public event SubscribtableAction ChannelCanceled;
	public void InvokeChannelCanceled() => ChannelCanceled?.Invoke();
	
	public event SubscribtableAction CastCanceled;
	public void InvokeCastCanceled() => CastCanceled?.Invoke();
	

	public event SubscribtableAction StateCompleted;
	public void InvokeStateCompleted() => StateCompleted?.Invoke();

	public event SubscribtableAction AbilityDisabled;
	public void DisableAbility(float duration)
	{
		// set duration of disable
		AbilityDisabled?.Invoke();
	}
	public event SubscribtableAction AbilityEnabled;
	public void EnableAbility() => AbilityEnabled?.Invoke();

    public event SubscribtableAction AbilityTriggerPressed;
    public event SubscribtableAction AbilityTriggerReleased;

    public void SetTrigger(bool trigger)
    {
        if (trigger)
        {
			Debug.Log("Invoking AbilityTriggerPressed.");
            AbilityTriggerPressed?.Invoke();
        }
        else
        {
            AbilityTriggerReleased?.Invoke();
        }
    }

	public event SubscribtableAction AbilityInterrupted;
	public void InterruptAbility() => AbilityInterrupted?.Invoke();

	#endregion
}
