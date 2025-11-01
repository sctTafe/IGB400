using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearTheCity_GameManager : MonoBehaviour
{
    [Header("Enemy Tracking")]
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private GameObject winPanel;

    [Header("Last Enemy Tracker")]
    [SerializeField] private GameObject lastEnemyPanel;
    [SerializeField] private TextMeshProUGUI lastEnemyDistanceText;
    [SerializeField] private Transform player;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeText;

    private float startTime;
    private float endTime;
    private bool gameStarted = false;
    private bool gameEnded = false;

    void Start()
    {
        // Initialize the game
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (lastEnemyPanel != null)
        {
            lastEnemyPanel.SetActive(false);
        }

        // Remove any null references from the initial list
        CleanEnemyList();

        // Start the timer
        startTime = Time.time;
        gameStarted = true;

        // Update the UI
        UpdateEnemyCountUI();
    }

    void Update()
    {
        if (gameStarted && !gameEnded)
        {
            // Clean the list and update UI every frame
            CleanEnemyList();
            UpdateEnemyCountUI();

            // Handle last enemy tracking
            UpdateLastEnemyTracking();

            // Check for win condition
            CheckWinCondition();
        }
    }

    /// <summary>
    /// Removes null references from the enemies list
    /// </summary>
    private void CleanEnemyList()
    {
        enemies.RemoveAll(enemy => enemy == null);
    }

    /// <summary>
    /// Updates the TextMeshPro UI with the current enemy count
    /// </summary>
    private void UpdateEnemyCountUI()
    {
        if (enemiesRemainingText != null)
        {
            enemiesRemainingText.text = $"Enemies: {enemies.Count}";
        }
    }

    /// <summary>
    /// Shows distance to last enemy when only one remains
    /// </summary>
    private void UpdateLastEnemyTracking()
    {
        if (enemies.Count == 1)
        {
            // Show the last enemy panel
            if (lastEnemyPanel != null && !lastEnemyPanel.activeSelf)
            {
                lastEnemyPanel.SetActive(true);
            }

            // Calculate and display distance
            if (player != null && enemies[0] != null && lastEnemyDistanceText != null)
            {
                float distance = Vector3.Distance(player.position, enemies[0].transform.position);
                lastEnemyDistanceText.text = $"{distance:F1}m";
            }
        }
        else
        {
            // Hide the panel when there's more than one enemy or none
            if (lastEnemyPanel != null && lastEnemyPanel.activeSelf)
            {
                lastEnemyPanel.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Checks if all enemies are defeated and triggers win condition
    /// </summary>
    private void CheckWinCondition()
    {
        if (enemies.Count == 0 && !gameEnded)
        {
            TriggerWin();
        }
    }

    /// <summary>
    /// Called when the player wins the game
    /// </summary>
    private void TriggerWin()
    {
        gameEnded = true;
        endTime = Time.time;

        float timeTaken = endTime - startTime;
        int score = CalculateScore(timeTaken);

        // Display win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Update score UI
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Score: {score}";
        }

        if (finalTimeText != null)
        {
            finalTimeText.text = $"Time: {FormatTime(timeTaken)}";
        }

        Debug.Log($"Victory! Time: {timeTaken:F2}s | Score: {score}");
    }

    /// <summary>
    /// Calculates score based on completion time
    /// Faster times = higher scores
    /// </summary>
    private int CalculateScore(float timeTaken)
    {
        // Base score of 10000, subtract 100 points per second
        // Adjust these values to your liking
        int baseScore = 10000;
        int penaltyPerSecond = 100;

        int score = baseScore - Mathf.RoundToInt(timeTaken * penaltyPerSecond);

        // Ensure score doesn't go below 0
        return Mathf.Max(0, score);
    }

    /// <summary>
    /// Formats time in MM:SS format
    /// </summary>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    /// <summary>
    /// Public method to manually add an enemy to tracking
    /// </summary>
    public void AddEnemy(GameObject enemy)
    {
        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            UpdateEnemyCountUI();
        }
    }

    /// <summary>
    /// Public method to manually remove an enemy from tracking
    /// </summary>
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            UpdateEnemyCountUI();
        }
    }

    /// <summary>
    /// Returns the current number of remaining enemies
    /// </summary>
    public int GetRemainingEnemyCount()
    {
        CleanEnemyList();
        return enemies.Count;
    }
}
