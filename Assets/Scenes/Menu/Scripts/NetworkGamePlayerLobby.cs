using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
	[SyncVar]
	private string displayName = "Loading...";


	private NetworkManagerCustom room;
	private NetworkManagerCustom Room
	{
		get
		{
			if (room != null) { return room ;}
			return room = NetworkManager.singleton as NetworkManagerCustom;
		}
	}

	public override void OnStartClient()
	{
		DontDestroyOnLoad(gameObject);
		Room.GamePlayers.Add(this);
	}

	public override void OnStopClient()
	{
		Room.GamePlayers.Remove(this);
	}

	[Server]
	public void SetDisplayName(string displayName)
	{
		this.displayName = displayName;
	}
}
