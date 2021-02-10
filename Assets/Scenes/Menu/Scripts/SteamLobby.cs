using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private NetworkManagerCustom _networkManager = null;
    [SerializeField] private GameObject _landingPagePanel = null;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private const string HostAddressKey = "HostAddress";

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        _landingPagePanel.transform.Find("HostOnSteam").gameObject.SetActive(true);


        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
        Debug.Log("Trying to reach Steam Network...");
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("Steam Lobby Creation failed");
            return;
        }

        Debug.Log("Steam lobby creation succesful. Host is" + SteamUser.GetSteamID().ToString());

        _networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        Debug.Log("Trying to join lobby (id: " + callback.m_steamIDLobby + ")");
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) 
        {
            // if we are the Host
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        _networkManager.networkAddress = hostAddress;

        Debug.Log("Joined " + hostAddress);

        _networkManager.StartClient();
    }
}
