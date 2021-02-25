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
        Debug.Log("hi");
        enabled = true;

        if (!string.IsNullOrEmpty(profile.Ability1))
        {
            GameObject abilityGameObject = AbilityDatabase.GetAbility(profile.Ability1, gameObject);
            abilityGameObject.transform.parent = transform;

            ability1 = abilityGameObject.GetComponent<Ability>();
            abilityGameObject.SetActive(true);

            PlayerInputHandler.OnLMB += ability1.SetTrigger;
        }

        if (!string.IsNullOrEmpty(profile.Ability2))
        {
            GameObject abilityGameObject = AbilityDatabase.GetAbility(profile.Ability2, gameObject);
            abilityGameObject.transform.parent = transform;

            ability2 = abilityGameObject.GetComponent<Ability>();

            /* TODO: set trigger */
        }

        if (!string.IsNullOrEmpty(profile.Ability3))
        {
            GameObject abilityGameObject = AbilityDatabase.GetAbility(profile.Ability3, gameObject);
            abilityGameObject.transform.parent = transform;

            ability3 = abilityGameObject.GetComponent<Ability>();
            
            /* TODO: set trigger */
        }
	}


    private void Update()
    {
        // if (ability1.stateMachine == null) Debug.Log("no statemachine");
        // if (ability1.stateMachine.currentState == null) Debug.Log("no currentState");
        // if (ability1 != null) Ability1State = ability1.stateMachine.currentState.GetType().ToString();
    }

    private void OnDestroy()
    {
        if (ability1 != null) PlayerInputHandler.OnLMB -= ability1.SetTrigger;
    }
}
