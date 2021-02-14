using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAbilityManager : NetworkBehaviour
{
    [SerializeField] private PlayerProfile profile = null;

    private Ability ability1;
    [SerializeField] public string Ability1State;
    private Ability ability2;
    private Ability ability3;

	public override void OnStartAuthority()
	{
        enabled = true;

        if (!string.IsNullOrEmpty(profile.Ability1))
        {
            ability1 = AbilityDatabase.GetAbility(profile.Ability1);
            ability1.SetOwner(gameObject);

            PlayerInputHandler.OnLMB += ability1.stateMachine.SetTrigger;
        }

        if (!string.IsNullOrEmpty(profile.Ability2))
        {
            ability2 = AbilityDatabase.GetAbility(profile.Ability2);
            ability2.SetOwner(gameObject);

            /* TODO: set trigger */
        }

        if (!string.IsNullOrEmpty(profile.Ability3))
        {
            ability3 = AbilityDatabase.GetAbility(profile.Ability3);
            ability3.SetOwner(gameObject);
            
            /* TODO: set trigger */
        }
	}

    // Debug
    private void Update()
    {
        if (ability1.stateMachine == null) Debug.Log("no statemachine");
        if (ability1.stateMachine.currentState == null) Debug.Log("no currentState");
        if (ability1 != null) Ability1State = ability1.stateMachine.currentState.GetType().ToString();
    }

    private void OnDestroy()
    {
        if (ability1 != null) PlayerInputHandler.OnLMB -= ability1.stateMachine.SetTrigger;
    }
}
