using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _nameInputField = null;
    [SerializeField] private Button _continueButton = null;
    
    public static string DisplayName { get; private set; }
    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetupInputField();

    private void SetupInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        _nameInputField.text = defaultName;

        SetPlayerName(defaultName);

    }

    public void SetPlayerName(string playerName)
    {
        _continueButton.interactable = !string.IsNullOrEmpty(playerName);
    }

    public void SavePlayerName()
    {
        DisplayName = _nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
