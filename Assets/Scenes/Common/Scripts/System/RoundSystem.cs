using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator _animator = null;

    private NetworkManagerCustom _room;

    private NetworkManagerCustom Room
    {
        get
        {
            if (_room != null) return _room;
            return NetworkManager.singleton as NetworkManagerCustom;
        }
    }

    public void CountDownEnded()
    {
        _animator.enabled = false;
    }

	#region Server

	public override void OnStartServer()
	{
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

    [ServerCallback]
    public void StartRound()
    {
        RpcStartRound();
    }

    [Server]
    private void CheckToStartRound(NetworkConnection conn)
    {
        if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) return;

        _animator.enabled = true;

        RpcStartCountdown();
    }

	#endregion

    #region Client

    [ClientRpc]
    private void RpcStartCountdown()
    {
        _animator.enabled = true;
    }
    
    [ClientRpc]
    private void RpcStartRound()
    {
        InputHandler.UnblockActionMap(ActionMapNames.Player);
        Debug.Log("Starting Round");
    }

    #endregion
}
