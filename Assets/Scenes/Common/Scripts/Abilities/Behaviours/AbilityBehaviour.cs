using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBehaviour : ScriptableObject
{
	private AbilityHandler _abilityHandler;

	public void Initialize(AbilityHandler abilityHandler, GameObject player = null)
	{
		_abilityHandler = abilityHandler;

		_abilityHandler.ChannelStarted += OnChannelStarted;
		_abilityHandler.ChannelCanceled += OnChannelCanceled;
		_abilityHandler.ChannelCompleted += OnChannelCompleted;
		
		_abilityHandler.CastStarted += OnCastStarted;
		_abilityHandler.CastCanceled += OnCastCanceled;
		_abilityHandler.CastCompleted += OnCastCompleted;
	}

	private void OnDestroy()
	{
		_abilityHandler.ChannelStarted -= OnChannelStarted;
		_abilityHandler.ChannelCanceled -= OnChannelCanceled;
		_abilityHandler.ChannelCompleted -= OnChannelCompleted;
		
		_abilityHandler.CastStarted -= OnCastStarted;
		_abilityHandler.CastCanceled -= OnCastCanceled;
		_abilityHandler.CastCompleted -= OnCastCompleted;
	}

	private void OnChannelStarted() {}
	
	private void OnChannelCanceled() {}
	
	private void OnChannelCompleted() {}
	
	private void OnCastStarted() {}
	
	private void OnCastCanceled() {}
	
	private void OnCastCompleted() {}
}
