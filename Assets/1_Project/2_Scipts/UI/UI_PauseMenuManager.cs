using UnityEngine;
using UnityEngine.UI;

public class UI_PauseMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    private bool isPaused = false;

    private void Start()
    {
        // Resume the game
        Time.timeScale = 1f;
        Debug.LogWarning("Game timeScale set to 1f on Start");

        // Ensure the pause menu is hidden at start
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        // Set up button listeners
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(fn_OpenPauseMenu);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(fn_ClosePauseMenu);
        }
    }

    private void OnDestroy()
    {
        // Clean up listeners to prevent memory leaks
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(fn_OpenPauseMenu);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(fn_ClosePauseMenu);
        }
    }

    public void fn_OpenPauseMenu()
    {
        if (isPaused) return;

        isPaused = true;

        // Show the pause menu
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    public void fn_ClosePauseMenu()
    {
        if (!isPaused) return;

        isPaused = false;

        // Hide the pause menu
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        // Resume the game
        Time.timeScale = 1f;
    }

    
}