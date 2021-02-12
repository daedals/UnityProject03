using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerCustom _networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject _landingPagePanel = null;
    [SerializeField] private TMP_InputField _ipAddressInputField = null;
    [SerializeField] private Button _joinButton = null;

    private void OnEnable()
    {
        // NetworkManagerCustom.OnClientConnected += HandleClientConnected;
        // NetworkManagerCustom.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerCustom.OnClientConnected -= HandleClientConnected;
        NetworkManagerCustom.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        // moved here from OnEnable
        NetworkManagerCustom.OnClientConnected += HandleClientConnected;
        NetworkManagerCustom.OnClientDisconnected += HandleClientDisconnected;

        string ipAddress = _ipAddressInputField.text;

        _networkManager.networkAddress = ipAddress;
        _networkManager.StartClient();

        _joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        Debug.Log("HandleClientConnect called in JoinLobbyMenu");
        _joinButton.interactable = true;

        gameObject.SetActive(false);
        _landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        _joinButton.interactable = true;
        _landingPagePanel.SetActive(true);
    }
}
