using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAbilityManager : NetworkBehaviour
{
    [SerializeField] private PlayerProfile profile = null;
    [SerializeField] private NetworkManagerCustom networkManager = null;

    private Ability ability1;
    [SerializeField] public string Ability1State;
    private Ability ability2;
    private Ability ability3;


    private static ulong uniqueRequesterId = 0;
    private Dictionary<ulong, GameObject> spawnRequester = new Dictionary<ulong, GameObject>();
    private List<System.Tuple<ulong, GameObject>> requestedInstances = new List<System.Tuple<ulong, GameObject>>();

	public override void OnStartAuthority()
	{
        enabled = true;

        if (!string.IsNullOrEmpty(profile.Ability1))
        {
            ability1 = AbilityDatabase.GetAbility(profile.Ability1);
            ability1.Initialize(gameObject);

            PlayerInputHandler.OnLMB += ability1.SetTrigger;
        }

        if (!string.IsNullOrEmpty(profile.Ability2))
        {
            ability2 = AbilityDatabase.GetAbility(profile.Ability2);
            ability2.Initialize(gameObject);

            /* TODO: set trigger */
        }

        if (!string.IsNullOrEmpty(profile.Ability3))
        {
            ability3 = AbilityDatabase.GetAbility(profile.Ability3);
            ability3.Initialize(gameObject);
            
            /* TODO: set trigger */
        }
	}


    private void Update()
    {
        if (ability1.stateMachine == null) Debug.Log("no statemachine");
        if (ability1.stateMachine.currentState == null) Debug.Log("no currentState");
        if (ability1 != null) Ability1State = ability1.stateMachine.currentState.GetType().ToString();
    }

    private void OnDestroy()
    {
        if (ability1 != null) PlayerInputHandler.OnLMB -= ability1.SetTrigger;
    }

    public void Add(ISpawnRequester requester, GameObject prefab)
    {
        ulong id = uniqueRequesterId++;
        spawnRequester[id] = prefab;

        requester.Id = id;
    }

    public void Remove(ISpawnRequester requester)
    {
        if (!spawnRequester.ContainsKey(requester.Id)) throw new System.Exception("Tried to remove unknown Spawn requester.");
        spawnRequester.Remove(requester.Id);
    }

    [Client]
    public void RequestPrefabSpawn(ISpawnRequester requester, Vector3 position, Quaternion rotation)
    {
        if (!spawnRequester.ContainsKey(requester.Id)) throw new System.Exception("oh no 2");
        CmdRequestSpawn(requester.Id, position, rotation);
    }

    [Command]
    private void CmdRequestSpawn(ulong id, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = spawnRequester[id];
        if (prefab == null) throw new System.Exception("oh no 3");

        GameObject instance = GameObject.Instantiate(prefab, position, rotation);
        NetworkServer.Spawn(instance, connectionToClient);

        TargetReturnInstance(connectionToClient, instance);

        // call target rpc
    }

    [TargetRpc]
    private void TargetReturnInstance(NetworkConnection target, GameObject instance)
    {
        // requester.ReceiveRequestedInstance(instance);
        Debug.Log("Instance spawned.");
    }
}
