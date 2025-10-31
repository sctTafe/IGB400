using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerPref_SettingsToggle : MonoBehaviour
{
    [Header("References")]
    public Button toggleButton;
    public TextMeshProUGUI buttonText; // Use Text if not using TextMeshPro

    [Header("Settings")]
    public string playerPrefsKey = "PlayerPrefKey_String";
    public bool defaultValue = true;

    protected bool currentValue;

    void Start()
    {
        fn_OnStart();

        // Update button text to match saved value
        UpdateButtonText();

        // Subscribe to button click
        if (toggleButton != null)
            toggleButton.onClick.AddListener(fn_ToggleSetting);
    }
    public virtual void fn_OnStart()
    {
        // Load saved value or use default
        currentValue = PlayerPrefs.GetInt(playerPrefsKey, defaultValue ? 1 : 0) == 1;
    }

    public virtual void fn_ToggleSetting()
    {
        // Toggle the value
        currentValue = !currentValue;

        // Save to PlayerPrefs
        PlayerPrefs.SetInt(playerPrefsKey, currentValue ? 1 : 0);
        PlayerPrefs.Save();

        // Update button text
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = currentValue ? "On" : "Off";
        }
    }

    // Public method to get the current value
    public bool GetValue()
    {
        return currentValue;
    }

    void OnDestroy()
    {
        if (toggleButton != null)
            toggleButton.onClick.RemoveListener(fn_ToggleSetting);
    }
}