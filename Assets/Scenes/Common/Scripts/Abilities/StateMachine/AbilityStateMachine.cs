using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityStateMachine : StateMachine
{
	public bool isChanneling = false;
	private float channelDuration;

	public bool isCasting = false;
	private float castDuration;

    public bool isOnCooldown = false;
	private float cooldownDuration;

	bool ChannelCompleted() => isChanneling;
	bool CastCompleted() => isCasting;
	bool CooldownCompleted() => isOnCooldown;


	bool Placeholder() => false;

	private void Awake()
	{
		var inactive = new Casting(this, castDuration);
		var channeling = new Channeling(this, channelDuration);
		var casting = new Casting(this, castDuration);
		var onCooldown = new OnCooldown(this, cooldownDuration);
		var disabled = new Channeling(this, channelDuration);

		AddTransition(inactive, channeling, Placeholder);
		AddTransition(channeling, casting, ChannelCompleted);
		AddTransition(casting, onCooldown, CastCompleted);
		AddTransition(onCooldown, inactive, Placeholder);

		
		AddAnyTransition(disabled, Placeholder);
	}
}
