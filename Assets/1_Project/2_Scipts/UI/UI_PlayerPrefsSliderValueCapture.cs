using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerPrefsSliderValueCapture : MonoBehaviour
{
    [Header("Slider Settings")]
    [SerializeField] private Slider slider;

    [Header("PlayerPrefs Settings")]
    [SerializeField] private string playerPrefsKey = "SliderValue";
    [SerializeField] private float defaultValue = 0.5f;

    private void Start()
    {
        // Ensure we have a slider reference
        if (slider == null)
        {
            slider = GetComponent<Slider>();
            if (slider == null)
            {
                Debug.LogError("No Slider component found! Please assign a slider.");
                return;
            }
        }

        // Load the saved value or use default
        float savedValue = PlayerPrefs.GetFloat(playerPrefsKey, defaultValue);
        slider.value = savedValue;

        // Subscribe to value changes
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Save the new value to PlayerPrefs
        PlayerPrefs.SetFloat(playerPrefsKey, value);
        PlayerPrefs.Save(); // Force save immediately
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    // Optional: Public method to get the current value
    public float GetValue()
    {
        return slider != null ? slider.value : defaultValue;
    }

    // Optional: Public method to reset to default
    public void ResetToDefault()
    {
        if (slider != null)
        {
            slider.value = defaultValue;
            PlayerPrefs.SetFloat(playerPrefsKey, defaultValue);
            PlayerPrefs.Save();
        }
    }
}