using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButtonLoader : MonoBehaviour
{
    [Header("UI References")]
    public Button triggerButton;

    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load (must be added to Build Settings).")]
    public string sceneToLoad;

    private void Start()
    {
        if (triggerButton != null)
            triggerButton.onClick.AddListener(LoadScene);
        else
            Debug.LogWarning($"{name}: No trigger button assigned!");

        if (string.IsNullOrEmpty(sceneToLoad))
            Debug.LogWarning($"{name}: No scene name assigned!");
    }

    private void OnDestroy()
    {
        if (triggerButton != null)
            triggerButton.onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"{name}: Scene name is empty. Please assign one in the inspector.");
        }
    }
}
