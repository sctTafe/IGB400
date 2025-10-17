using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SceneSwitcher : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button gameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button aboutButton;

    [Header("Scene Names (must match Build Settings)")]
    [SerializeField] private string mainMenuScene = "0_Menu";
    [SerializeField] private string gameScene = "1_Game";
    [SerializeField] private string settingsScene = "2_Settings";
    [SerializeField] private string aboutScene = "3_About";

    private void Awake()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => LoadScene(mainMenuScene));

        if (gameButton != null)
            gameButton.onClick.AddListener(() => LoadScene(gameScene));

        if (settingsButton != null)
            settingsButton.onClick.AddListener(() => LoadScene(settingsScene));

        if (aboutButton != null)
            aboutButton.onClick.AddListener(() => LoadScene(aboutScene));
    }

    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("UI_SceneSwitcher: Scene name is empty!");
        }
    }
}
