using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AbilityInputProcessor : NetworkBehaviour
{
    [Header("Abilities")]
    [SerializeField] private Ability _lmbAbility = null;
    private AbilityHandler _lmbAbilityHandler;
    
	public override void OnStartAuthority()
	{
        enabled = true;

        if (_lmbAbility != null)
        {
            _lmbAbilityHandler = new AbilityHandler(_lmbAbility.ChannelTime, _lmbAbility.CastTime);
            PlayerInputHandler.OnLMB += _lmbAbilityHandler.SetTrigger;

            // initialize behaviours
        }
	}

    private void OnDestroy()
    {
        if (_lmbAbility != null) PlayerInputHandler.OnLMB -= _lmbAbilityHandler.SetTrigger;
    }
}
