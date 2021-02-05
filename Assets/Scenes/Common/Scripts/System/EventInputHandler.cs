using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EventInputHandler : MonoBehaviour, GameControls.IGlobalEventsActions
{
    [Header("References")]
    [SerializeField] private GameControls _controls;
    
    private void OnEnable()
    {
        _controls = new GameControls();
        _controls.GlobalEvents.SetCallbacks(this);
        _controls.Enable();
    }

    public void OnResetScene(InputAction.CallbackContext context)
    {
        Debug.Log("Reloading Scene.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

	
