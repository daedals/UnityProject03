using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using System;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab = null;

    private static List<Transform> _spawnPoints = new List<Transform>();
    private int _nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        _spawnPoints.Add(transform);

        _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

	public override void OnStartClient()
	{
        /* TODO: this is not elegant, move to dedicated script */
        InputHandler.BlockActionMap(ActionMapNames.Player);
	}

	
    public static void RemoveSpawnPoint(Transform transform) => _spawnPoints.Remove(transform);

	public override void OnStartServer() => NetworkManagerCustom.OnServerReadied += SpawnPlayer;

    [ServerCallback]
    private void OnDestroy() => NetworkManagerCustom.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = _spawnPoints.ElementAtOrDefault(_nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {_nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(_playerPrefab, _spawnPoints[_nextIndex].position, _spawnPoints[_nextIndex].rotation);

        Debug.Log("Spawning in player");

        NetworkServer.Spawn(playerInstance, conn);

        playerInstance.GetComponent<HealthHandler>().PlayerDeath += OnPlayerDeath;

        _nextIndex++;
    }

    private void OnPlayerDeath(uint netId)
    {
        GameObject playerInstance = NetworkIdentity.spawned[netId].gameObject;
        NetworkServer.UnSpawn(playerInstance);
    }
}
