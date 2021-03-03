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
        AbilityDatabase.AbilityDataLoaded += OnAbilityDataLoaded;
	}

    [Client]
    public void OnAbilityDataLoaded()
    {
        Debug.Log("Fetching abilities for player.");

        uint netId = GetComponent<NetworkIdentity>().netId;

        if (!string.IsNullOrEmpty(profile.Ability1)) 
        {
            AbilityDatabase.Instance.CmdSetupAbility(netId, profile.Ability1);
        }

        /* TODO: repeat for other abilities */
    }


    [TargetRpc]
    public void TargetActivateAbility(NetworkConnection conn, string abilityName, uint netId) => ActivateAbility(abilityName, netId);

    public void ActivateAbility(string abilityName, uint netId)
    {
        Debug.Log("PlayerAbilityManager AcitvateAbility");
        
        GameObject obj = NetworkIdentity.spawned[netId].gameObject;

        if (profile.Ability1 == abilityName)
        {
            ability1 = obj.GetComponent<Ability>();

            Debug.Log("Fetched " + abilityName);

            PlayerInputHandler.OnLMB += ability1.SetTrigger;
            ability1.Initialize();
        }

        /* TODO: repeat for other abilities */
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
        if (ability1 != null) PlayerInputHandler.OnLMB -= ability1.SetTrigger;
    }
}
