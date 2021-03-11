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
        
        Debug.Log("Fetching abilities for player.");

        AbilityDatabase.AllPlayersConnected += OnAllPlayersConnected;
	}

    [Client]
    public void OnAllPlayersConnected()
    {
        Debug.Log("Fetching abilities for player.");

        uint netId = GetComponent<NetworkIdentity>().netId;

        if (!string.IsNullOrEmpty(profile.Ability1)) 
        {
            Debug.Log("Fetching " + profile.Ability1);
            AbilityDatabase.Instance.CmdSetupAbility(netId, profile.Ability1);
        }

        /* TODO: repeat for other abilities */
    }

    public void ActivateAbility(string abilityName)
    {
        Debug.Log("PlayerAbilityManager AcitvateAbility");

        if (profile.Ability1 == abilityName)
        {
            Transform obj = transform.Find(abilityName);

            if (obj == null) throw new System.Exception($"Ability {abilityName} could not be found in current context.");

            ability1 = obj.GetComponent<Ability>();

            Debug.Log("Fetched " + abilityName);

            PlayerInputHandler.OnLMB += ability1.SetTrigger;
            ability1.Initialize();

            return;
        }

        /* TODO: repeat for other abilities */

        throw new System.Exception("Ability activiation for player failed.");
    }

    private void Update()
    {
        if (ability1 != null)
        {
            if (ability1.stateMachine == null) Debug.Log("no statemachine");
            if (ability1.stateMachine.currentState == null) Debug.Log("no currentState");
            Ability1State = ability1.stateMachine.currentState.GetType().ToString();
        }
    }

    private void OnDestroy()
    {
        AbilityDatabase.AllPlayersConnected -= OnAllPlayersConnected;

        if (ability1 != null) PlayerInputHandler.OnLMB -= ability1.SetTrigger;
    }
}
