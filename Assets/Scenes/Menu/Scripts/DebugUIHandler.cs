using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUIHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _text = null;

    private static DebugUIHandler _instance;

    private string _log;
    private Queue<string> _logQueue = new Queue<string>();

    private void Awake()
    {
        // singleton logic
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string msg, string stackTrace, LogType type)
    {
        string color;

        switch (type)
        {
        case LogType.Log:
            color = "white";
            break;
        case LogType.Warning:
            color = "yellow";
            break;
        case LogType.Error:
            color = "red";
            break;
        default:
            color = "white";
            break;
        }

        bool a = msg.Contains("eference");

        string log = "<color=" + color + ">[" + type + "] " + msg + (a ? "\n" + stackTrace : "") + "</color=" + color + ">\n";

        _logQueue.Enqueue(log);
        _log = string.Empty;

        foreach (string item in _logQueue)
        {
            _log += item;
        }

        _text.text = _log;
    }
}
