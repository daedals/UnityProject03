using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerPrefabPool : NetworkBehaviour
{
    [SerializeField] private int maximumSpawnedInstances = 16;
    private static ulong uniqueRequesterId = 1;
    
    private Dictionary<ulong, GameObject> spawnRequester = new Dictionary<ulong, GameObject>();
    private List<System.Tuple<ulong, GameObject>> spawnedInstances;

	public override void OnStartAuthority()
	{
        enabled = true;

        // initialize pool of spawned instances to null references. PlayerPrefabPool does not change size to ensure faithfullness to the maximum number of allowed instances per player
        spawnedInstances = new List<System.Tuple<ulong, GameObject>>(Enumerable.Repeat<System.Tuple<ulong, GameObject>>(null, maximumSpawnedInstances));
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

        for (int i = 0; i < maximumSpawnedInstances; i++)
        {
            if (spawnedInstances[i].Item1 == requester.Id)
            {
                CmdDespawnInstance(i);
            }
        }

        spawnRequester.Remove(requester.Id);
    }


    [Client]
    public int RequestPrefabSpawn(ISpawnRequester requester, Vector3 position, Quaternion rotation)
    {
        if (!spawnRequester.ContainsKey(requester.Id)) throw new System.Exception("oh no 2");

        int ticket = GetUnusedTicket();

        if (ticket == -1) throw new System.Exception("Maximum number of prefabs for this player reached.");

        CmdRequestSpawn(requester.Id, ticket, position, rotation);

        return ticket;
    }

    private int GetUnusedTicket()
    {
        return spawnedInstances.Select<System.Tuple<ulong, GameObject>, bool>(x => x == null).ToList().IndexOf(true);
    }


    [Command]
    private void CmdRequestSpawn(ulong id, int ticket, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = spawnRequester[id];
        if (prefab == null) throw new System.Exception("oh no 3");

        GameObject instance = GameObject.Instantiate(prefab, position, rotation);
        NetworkServer.Spawn(instance, connectionToClient);

        spawnedInstances[ticket] = new System.Tuple<ulong, GameObject>(id, instance);
    }

    public System.Tuple<ulong, GameObject> RequestSpawnedInstance(ISpawnRequester requester, int ticket)
    {
        if (spawnedInstances[ticket] == null) 
        {
            Debug.Log("The requested instance has not been spawned yet.");
            return null;
        }

        if (spawnedInstances[ticket].Item1 != requester.Id)
        {
            throw new System.Exception("Requester is not the owner of this instance.");
        }

        return spawnedInstances[ticket];
    }

    [Client]
    public void RequestInstanceDespawn(ISpawnRequester requester, int ticket)
    {
        Debug.Log("Trying to despawn instance.");

        if (spawnedInstances[ticket] == null) 
        {
            throw new System.Exception("The instance could not be despawned because it does not exist.");
        }

        if (spawnedInstances[ticket].Item1 != requester.Id)
        {
            throw new System.Exception("Requester is not the owner of this instance.");
        }

        CmdDespawnInstance(ticket);
    }

    [Command]
    private void CmdDespawnInstance(int ticket)
    {
        Destroy(spawnedInstances[ticket].Item2);
        spawnedInstances[ticket] = null;
    }
}
