using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityStateMachine : StateMachine
{
	private float channelDuration;
	private float castDuration;
	private float cooldownDuration;
	private float interruptionDuration;
	private float disableDuration;

	private readonly List<BaseBehaviour> behaviours = new List<BaseBehaviour>();

	public GameObject owner;

	/* placeholder - create constructor */

	public AbilityStateMachine(List<BaseBehaviour> behaviours, AbilityTemplate template, GameObject owner) : base(owner?.GetComponent<PlayerAbilityManager>())
	{
		this.behaviours = behaviours;
		this.owner = owner;

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
		AddTransition(inactive, channeling, AbilityTriggerPressed);

		AddTransition(channeling, inactive, AbilityTriggerReleased);
		AddTransition(channeling, interrupted, AbilityInterrupted);
		AddTransition(channeling, casting, StateCompleted);

		AddTransition(casting, interrupted, AbilityInterrupted);
		AddTransition(casting, onCooldown, StateCompleted);

		AddTransition(onCooldown, inactive, StateCompleted);

		AddTransition(interrupted, inactive, StateCompleted);
		
		AddTransition(disabled, inactive, AbilityEnabled);
		AddAnyTransition(disabled, AbilityDisabled);

		Debug.Log(owner != null);

		// set starting position
		SetState(inactive);
	}

	#region Events
	private event Action ChannelCanceled;
	public void InvokeChannelCanceled() => ChannelCanceled?.Invoke();
	
	private event Action CastCanceled;
	public void InvokeCastCanceled() => CastCanceled?.Invoke();
	

	private event Action StateCompleted;
	public void InvokeStateCompleted() => StateCompleted?.Invoke();

	private event Action AbilityDisabled;
	public void DisableAbility(float duration)
	{
		// set duration of disable
		AbilityDisabled?.Invoke();
	}
	private event Action AbilityEnabled;
	public void EnableAbility() => AbilityEnabled?.Invoke();

    private event Action AbilityTriggerPressed;
    private event Action AbilityTriggerReleased;

    public void SetTrigger(bool trigger)
    {
        if (trigger)
        {
            AbilityTriggerPressed?.Invoke();
        }
        else
        {
            AbilityTriggerReleased?.Invoke();
        }
    }

	private event Action AbilityInterrupted;
	public void InterruptAbility() => AbilityInterrupted?.Invoke();

	#endregion
}
