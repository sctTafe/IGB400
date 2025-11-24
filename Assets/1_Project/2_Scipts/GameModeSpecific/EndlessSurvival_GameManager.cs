using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Scott.Barley.v2;

public class EndlessSurvival_GameManager : MonoBehaviour
{
    [Header("Enemy Tracking")]
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TextMeshProUGUI finalKillsText;
    [SerializeField] private int pointsPerKill = 100;

    private float startTime;
    private float endTime;
    private int totalKills = 0;
    private bool gameStarted = false;
    private bool gameEnded = false;

    PlayerStats_Singleton playerStats_Singleton;

    void Start()
    {
        playerStats_Singleton = PlayerStats_Singleton.Instance;
        Time.timeScale = 1f;

        // Initialize the game
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Remove any null references from the initial list
        CleanEnemyList();

        // Start the timer
        startTime = Time.time;
        gameStarted = true;

        // Update the UI
        UpdateKillCountUI();
    }

    void Update()
    {
        if (gameStarted && !gameEnded)
        {
            // Clean the list and update UI every frame
            CleanEnemyList();
            UpdateKillCountUI();

            // Check for game over condition
            CheckGameOverCondition();
        }
    }

    /// <summary>
    /// Removes null references from the enemies list and counts kills
    /// </summary>
    private void CleanEnemyList()
    {
        int previousCount = enemies.Count;
        enemies.RemoveAll(enemy => enemy == null);
        int removedCount = previousCount - enemies.Count;

        // Track kills when enemies are removed
        if (removedCount > 0)
        {
            totalKills += removedCount;
        }
    }

    /// <summary>
    /// Updates the TextMeshPro UI with the current kill count
    /// </summary>
    private void UpdateKillCountUI()
    {
        if (enemiesKilledText != null)
        {
            enemiesKilledText.text = $"Kills: {totalKills}";
        }
    }

    /// <summary>
    /// Checks if player is dead and triggers game over
    /// </summary>
    private void CheckGameOverCondition()
    {
        if (playerStats_Singleton != null && playerStats_Singleton.playerIsDead && !gameEnded)
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Called when the player dies
    /// </summary>
    private void TriggerGameOver()
    {
        gameEnded = true;
        endTime = Time.time;
        Time.timeScale = 0f;

        float survivalTime = endTime - startTime;
        int score = CalculateScore(survivalTime, totalKills);

        int bonusScore = UI_Score_Singelton.Instance.fn_GetScore();
        score += bonusScore;

        // Display game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Update score UI
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Score: {score}";
        }

        if (finalTimeText != null)
        {
            finalTimeText.text = $"Time: {FormatTime(survivalTime)}";
        }

        if (finalKillsText != null)
        {
            finalKillsText.text = $"Kills: {totalKills}";
        }

        Debug.Log($"Game Over! Survived: {survivalTime:F2}s | Kills: {totalKills} | Score: {score}");
    }

    /// <summary>
    /// Calculates score based on survival time and kills
    /// </summary>
    private int CalculateScore(float survivalTime, int kills)
    {
        // Score = (survival time in seconds * 10) + (kills * pointsPerKill)
        int timeScore = Mathf.RoundToInt(survivalTime * 10f);
        int killScore = kills * pointsPerKill;

        return timeScore + killScore;
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
        }
    }

    /// <summary>
    /// Public method to manually remove an enemy from tracking and count as kill
    /// </summary>
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            totalKills++;
            UpdateKillCountUI();
        }
    }

    /// <summary>
    /// Returns the current number of active enemies
    /// </summary>
    public int GetActiveEnemyCount()
    {
        CleanEnemyList();
        return enemies.Count;
    }

    /// <summary>
    /// Returns the total number of kills
    /// </summary>
    public int GetTotalKills()
    {
        return totalKills;
    }

    /// <summary>
    /// Returns the current survival time
    /// </summary>
    public float GetSurvivalTime()
    {
        if (gameEnded)
        {
            return endTime - startTime;
        }
        return Time.time - startTime;
    }
}