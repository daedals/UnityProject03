using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using System;

/*##########################################################################################################################

This Database is can only exist as a singleton and is instanciated by the NetworkManagerCustom when the server is started.
It loads all Assets of "AbilityTemplates" (datacontainer inheriting from ScriptableObject) in "Resource/Abilities".

Each asset is then instantiated as an "Ability" (Ability.cs) and cached in a dict where the key is the variable Name of the 
template. The Player prefab has a component "PlayerAbilityManager" that clones all relevant Abilities (dictated by Player
prefabs component "PlayerProfile") from the Database.

##########################################################################################################################*/

public class AbilityDatabase : NetworkBehaviour
{
    private Dictionary<string, AbilityTemplate> Database = new Dictionary<string, AbilityTemplate>();

    [SerializeField] private GameObject abilityTemplate;
    private const string abilityTemplateIdentifier = "MecanimAbilityTemplate";

    public static AbilityDatabase Instance { get; private set; }

    public static event Action AllPlayersConnected;

    private NetworkManagerCustom _room;
    private NetworkManagerCustom Room
    {
        get
        {
            if (_room != null) return _room;
            return NetworkManager.singleton as NetworkManagerCustom;
        }
    }

	public override void OnStartClient()
	{
        if (Instance) throw new System.Exception("There is already an Instance of AbilityDatabase.");

        Instance = this;

        LoadAbilityData();
	}
	

    [Server]
    public override void OnStartServer()
    {
        abilityTemplate = Room.spawnPrefabs.Find(prefab => prefab.name == abilityTemplateIdentifier);

        if (abilityTemplate == null) throw new System.Exception("No dummy ability found.");

        LoadAbilityData();

        NetworkManagerCustom.OnServerStopped += CleanUpServer;
        NetworkManagerCustom.OnServerReadied += CheckToStartRound;
	}

    [ServerCallback]
    private void OnDestroy() => CleanUpServer();

    [Server]
    private void CleanUpServer()
    {
        NetworkManagerCustom.OnServerStopped -= CleanUpServer;
        NetworkManagerCustom.OnServerReadied -= CheckToStartRound;
    }

    [Server]
    private void CheckToStartRound(NetworkConnection conn)
    {
        if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) return;

        Debug.Log("All Players connected. Setting up abilities.");

        RpcBroadcastPlayersConnected();
    }

    void LoadAbilityData()
    {
        var abilities = Resources.LoadAll<AbilityTemplate>("Abilities") as AbilityTemplate[];

        foreach (AbilityTemplate template in abilities)
        {
            Database[template.name] = template;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetupAbility(uint netId, string abilityName)
    {
        if (!Database.ContainsKey(abilityName)) throw new System.Exception("Unknown ability " + abilityName);

        if (NetworkIdentity.spawned.TryGetValue(netId, out NetworkIdentity player))
        {
            Debug.Log("Spawning in Ability Object for player " + player.connectionToClient);

            GameObject abilityObj = Instantiate(abilityTemplate);

            NetworkServer.Spawn(abilityObj, player.connectionToClient);

            uint abilityNetId = abilityObj.GetComponent<NetworkIdentity>().netId;

            if (!Database[abilityName].Parse(abilityNetId, player.netId))
                throw new System.Exception("Parsing Ability from template failed on Server.");

            RpcSetupAbility(abilityName, abilityNetId, player.netId);

            abilityObj.GetComponent<MecanimAbility>().enabled = true;
            RpcEnableAbility(abilityNetId);

            TargetSetupAbility(player.connectionToClient, player.netId, abilityName);

            return;
        }

        throw new System.Exception("No player instance found wiht netId " + netId);
    }

    [ClientRpc]
    private void RpcEnableAbility(uint netId) => NetworkIdentity.spawned[netId].GetComponent<MecanimAbility>().enabled = true;

    [ClientRpc]
    private void RpcSetupAbility(string abilityName, uint templateNetId, uint playerNetId)
    {
        if (!Database[abilityName].Parse(templateNetId, playerNetId))
            throw new System.Exception("Parsing Ability from template failed on Client.");
    }

    [TargetRpc]
    private void TargetSetupAbility(NetworkConnection conn, uint netId, string abilityName)
    {
        if (NetworkIdentity.spawned.TryGetValue(netId, out NetworkIdentity target))
        {
            target.GetComponent<PlayerAbilityManager>().ActivateAbility(abilityName);
        }
    }

    [ClientRpc]
    private void RpcBroadcastPlayersConnected() => AllPlayersConnected?.Invoke();
}
