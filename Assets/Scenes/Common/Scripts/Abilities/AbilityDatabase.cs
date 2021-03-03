using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

/*##########################################################################################################################

This Database is can only exist as a singleton and is instanciated by the NetworkManagerCustom when the server is started.
It loads all Assets of "AbilityTemplates" (datacontainer inheriting from ScriptableObject) in "Resource/Abilities".

Each asset is then instantiated as an "Ability" (Ability.cs) and cached in a dict where the key is the variable Name of the 
template. The Player prefab has a component "PlayerAbilityManager" that clones all relevant Abilities (dictated by Player
prefabs component "PlayerProfile") from the Database.

##########################################################################################################################*/

public class AbilityDatabase : NetworkBehaviour
{
    private Dictionary<string, GameObject> Database = new Dictionary<string, GameObject>();
    private SyncDictionary<string, System.Guid> AssetIds = new SyncDictionary<string, System.Guid>();

    public static AbilityDatabase Instance { get; private set; }

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
	}
	

    [Server]
    public override void OnStartServer()
    {
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

        RpcLoadAbilityData();
    }

    void LoadAbilityData()
    {
        var abilities = Resources.LoadAll<AbilityTemplate>("Abilities") as AbilityTemplate[];

        foreach (AbilityTemplate template in abilities) if (!Database.ContainsKey(template.name))
        {
            GameObject abilityObject = template.CreateAbilityObject();
            abilityObject.transform.parent = this.transform;

            if (NetworkServer.active)
            {
                Room.spawnPrefabs.Add(abilityObject);
            }

            Database[template.Name] = abilityObject;

            ClientScene.RegisterPrefab(abilityObject, GetAssetId(template.name));

            Debug.Log("Loaded Ability: " + template.Name + " " + abilityObject.GetComponent<NetworkIdentity>().assetId);
        }
    }

    private System.Guid GetAssetId(string name)
    {
        if (AssetIds.ContainsKey(name)) return AssetIds[name];

        if (NetworkServer.active)
        {
            System.Guid assetId = System.Guid.NewGuid();
            AssetIds[name] = assetId;

            return assetId;
        }

        throw new System.Exception("Unknown asset id.");
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetupAbility(uint netId, string abilityName)
    {
        if (!Database.ContainsKey(abilityName)) throw new System.Exception("Unknown ability " + abilityName);

        GameObject player = NetworkIdentity.spawned[netId].gameObject;

        if (player == null) throw new System.Exception("No player instance found wiht netId " + netId);

        GameObject abilityObj = Instantiate(Database[abilityName], Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(abilityObj, player.GetComponent<NetworkIdentity>().connectionToClient);

        RpcSetupAbility(netId, abilityObj.GetComponent<NetworkIdentity>().netId);

        player.GetComponent<PlayerAbilityManager>().TargetActivateAbility(connectionToClient, abilityName, abilityObj.GetComponent<NetworkIdentity>().netId);
    }

    [ClientRpc]
    private void RpcSetupAbility(uint playerNetId, uint abilityNetId)
    {
        GameObject player = NetworkIdentity.spawned[playerNetId].gameObject;
        GameObject ability = NetworkIdentity.spawned[abilityNetId].gameObject;

        ability.transform.parent = player.transform;
    }

    public static event System.Action AbilityDataLoaded;

    [ClientRpc]
    public void RpcLoadAbilityData()
    {
        LoadAbilityData();

        AbilityDataLoaded?.Invoke();
    }
}
