using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private NetworkManagerCustom _networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject _landingPagePanel = null;


    public void HostLobby()
    {
        _networkManager.StartHost();

        _landingPagePanel.SetActive(false);
    }

    public void ButtonExit()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    // currently not used, because roomplayer has no access
    public void GoBackAndReactivateLandingPanel()
    {
        _landingPagePanel.SetActive(true);

        if (NetworkServer.active) _networkManager.StopServer();
        
        _networkManager.StopClient();
    }
}
