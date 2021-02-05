using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
	[SerializeField] private GameObject _lobbyUI = null;
	[SerializeField] private TMP_Text[] _playerNameText = new TMP_Text[4];
	[SerializeField] private TMP_Text[] _playerReadyText = new TMP_Text[4];
	[SerializeField] private Button startGameButton = null;

	[SyncVar(hook = nameof(HandleDisplayNameChanged))]
	public string DisplayName = "Loading...";
	[SyncVar(hook = nameof(HandleReadyStatusChanged))]
	public bool IsReady = false;

	private bool isLeader;

	public bool IsLeader
	{
		set
		{
			isLeader = value;
			startGameButton.gameObject.SetActive(value);
		}
	}

	private NetworkManagerCustom room;
	private NetworkManagerCustom Room
	{
		get
		{
			if (room != null) { return room ;}
			return room = NetworkManager.singleton as NetworkManagerCustom;
		}
	}

	public override void OnStartAuthority()
	{
		CmdSetDisplayName(PlayerNameInput.DisplayName);
		_lobbyUI.SetActive(true);
	}

	public override void OnStartClient()
	{
		Room.RoomPlayers.Add(this);

		UpdateDisplay();
	}

	public override void OnStopClient()
	{
		Room.RoomPlayers.Remove(this);

		UpdateDisplay();
	}

	public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
	public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

	private void UpdateDisplay()
	{
		if (!hasAuthority)
		{
			foreach (var player in Room.RoomPlayers)
			{
				if (player.hasAuthority)
				{
					player.UpdateDisplay();
					break;
				}
			}

			return;
		}

		for (int i = 0; i < _playerNameText.Length; i++)
		{
			_playerNameText[i].text = "Waiting for Player...";
			_playerReadyText[i].text = string.Empty;
		}

		for (int i = 0; i < Room.RoomPlayers.Count; i++)
		{
			_playerNameText[i].text = Room.RoomPlayers[i].DisplayName;
			_playerReadyText[i].text = Room.RoomPlayers[i].IsReady ?
				"<color=green>Ready</color=green>" :
				"<color=red>Not ready</color=red>";
		}
	}

	public void HandleReadyToStart(bool readyToStart)
	{
		if (!isLeader) return;

		startGameButton.interactable = readyToStart;
	}

	[Command]
	private void CmdSetDisplayName(string displayName)
	{
		// logic to validate name
		DisplayName = displayName;
	}

	[Command]
	public void CmdReadyUp()
	{
		IsReady = !IsReady;

		Room.NotifyPlayersOfReadyState();
	}

	[Command]
	public void CmdStartGame()
	{
		if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return;}

		Room.StartGame();
	}

	[Command]
	public void CmdLeaveLobby()
	{
		if(isLeader)
		{
			Room.StopHost();
		}
		else
		{
			Room.StopClient();
			Room.RoomPlayers.Clear();
		}

		// set landing panel to active
	}
}
