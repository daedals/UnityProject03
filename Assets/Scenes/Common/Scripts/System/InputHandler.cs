using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    /*
    keeps track of players valid inputs
    certain input maps can be blocked
    creates global controls instance (scene bound)
    */
    private static readonly IDictionary<string, int> _mapStates = new Dictionary<string, int>();

    private static GameControls _controls;
    public static GameControls Controls
    {
        get
        {
            if (_controls != null) return _controls;
            return _controls = new GameControls();
        }
    }

    private void Awake()
    {
        if (_controls != null) {  return; }
        _controls = new GameControls();
    }

    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();
    private void OnDestroy() => _controls = null;

    public static void BlockActionMap(string mapName)
    {
        _mapStates.TryGetValue(mapName, out int value);
        _mapStates[mapName] = value + 1;

        UpdateMapState(mapName);
    }

    public static void UnblockActionMap(string mapName)
    {
        _mapStates.TryGetValue(mapName, out int value);
        _mapStates[mapName] = Mathf.Max(value - 1, 0);

        UpdateMapState(mapName);
    }

    private static void UpdateMapState(string mapName)
    {
        int value = _mapStates[mapName];

        if (value > 0)
        {

            Controls.asset.FindActionMap(mapName).Disable();
            return;
        }

        Controls.asset.FindActionMap(mapName).Enable();
    }
}
