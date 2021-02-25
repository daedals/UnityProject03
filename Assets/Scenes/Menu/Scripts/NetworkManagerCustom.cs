using Mirror;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerCustom : NetworkManager
{
    [SerializeField] private int _minPlayers = 1;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Maps")]
    [SerializeField] private int _numperOfRounds = 1;
    [SerializeField] private MapSet _mapSet = null;

    [Header("Menu")]
    [SerializeField] private NetworkRoomPlayerLobby _roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby _gamePlayerPrefab = null;

    // all systems
    [SerializeField] private GameObject _playerSpawnSystem = null;
    [SerializeField] private GameObject _roundSystem = null;
    [SerializeField] private GameObject _abilityDatabase = null;

    private MapHandler _mapHandler;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();


    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;


	public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

	public override void OnStartClient()
	{
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach(var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
	}

	// public override void OnDestroy()
	// {
	// 	base.OnDestroy();
        
    //     var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

    //     foreach(var prefab in spawnablePrefabs)
    //     {
    //         ClientScene.UnregisterPrefab(prefab);
    //     }
	// }

	public override void OnClientConnect(NetworkConnection conn)
	{
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
	}
	public override void OnClientDisconnect(NetworkConnection conn)
	{
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(_roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

            NotifyPlayersOfReadyState();
        }
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnClientDisconnect(conn);
	}

	public override void OnStopServer()
	{
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
	}

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < _minPlayers) return false;

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) return false;
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart()) return;

            _mapHandler = new MapHandler(_mapSet, _numperOfRounds);

            ServerChangeScene(_mapHandler.NextMap);
        }
    }

	public override void ServerChangeScene(string newSceneName)
	{
        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
        {
            var conn = RoomPlayers[i].connectionToClient;
            var gamePlayerInstance = Instantiate(_gamePlayerPrefab);
            gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

            NetworkServer.Destroy(conn.identity.gameObject);

            NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
        }

		base.ServerChangeScene(newSceneName);
	}

	public override void OnServerSceneChanged(string sceneName)
	{
        if (sceneName.StartsWith("Assets/Scenes/SampleMap"))
        {
            GameObject abilityDatabaseInstance = Instantiate(_abilityDatabase);
            NetworkServer.Spawn(abilityDatabaseInstance);

            GameObject playerSpawnSystemInstance = Instantiate(_playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject roundSystemInstance = Instantiate(_roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
        }

		base.OnServerSceneChanged(sceneName);
	}


	public override void OnServerReady(NetworkConnection conn)
	{
		base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
	}
}
