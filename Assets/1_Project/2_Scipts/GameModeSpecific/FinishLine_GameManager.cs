using UnityEngine;
using TMPro;

namespace Scott.Barley.v2
{
    public class FinishLine_GameManager : MonoBehaviour
    {
        [Header("References")]
        public Transform player;
        public Transform endPoint;
        public TMP_Text distanceText;
        public GameObject winPanel;
        public TMP_Text finalTimeText;
        public TMP_Text finalScoreText;

        [Header("Game Settings")]
        [Tooltip("Minimum distance from the endpoint to trigger the win.")]
        public float winDistanceThreshold = 10f;

        [Tooltip("Maximum base score for perfect performance.")]
        public int maxScore = 1000;

        [Tooltip("Expected player speed (m/s) for target time calculation.")]
        public float targetSpeed = 5f;

        [Tooltip("Adjusts the calculated target time — 1.0 = normal, >1 gives more time, <1 gives less.")]
        [Range(0.5f, 2.0f)]
        public float targetTimeMultiplier = 1.0f;

        private bool gameEnded = false;
        private float startDistance;
        private float targetTime;

        private void Start()
        {
            if (player == null || endPoint == null)
            {
                Debug.LogError("GameManager: Missing Player or EndPoint reference!");
                enabled = false;
                return;
            }

            if (winPanel != null)
                winPanel.SetActive(false);

            // Work out how far the player starts from the goal
            startDistance = Vector3.Distance(player.position, endPoint.position);

            // Determine expected (target) time and apply multiplier
            targetTime = (startDistance / targetSpeed) * targetTimeMultiplier;

            Debug.Log($"🎯 Target time: {targetTime:F2}s (Distance {startDistance:F1}m @ {targetSpeed:F1}m/s, Multiplier {targetTimeMultiplier:F2})");
        }

        private void Update()
        {
            if (gameEnded) return;

            float distance = Vector3.Distance(player.position, endPoint.position);
            if (distanceText != null)
                distanceText.text = $"Distance: {distance:F1}m";

            if (distance <= winDistanceThreshold)
                EndGame();
        }

        private void EndGame()
        {
            gameEnded = true;

            float timeTaken = UI_GameTimer.Instance.fn_GetElapsedTime();
            int bonusScore = UI_Score_Singelton.Instance.fn_GetScore();

            // Score decreases if player slower than target time
            float performance = Mathf.Clamp01(targetTime / timeTaken);
            int timeScore = Mathf.RoundToInt(maxScore * performance);

            int finalScore = timeScore + bonusScore;

            Debug.Log($"🏁 You win! Time: {timeTaken:F2}s | Target: {targetTime:F2}s | " +
                      $"Performance: {performance:P0} | Base: {timeScore} | Bonus: {bonusScore} | Final: {finalScore}");

            if (winPanel != null)
                winPanel.SetActive(true);

            if (finalTimeText != null)
                finalTimeText.text = $"Time: {timeTaken:F2}s (Target: {targetTime:F2}s)";

            if (finalScoreText != null)
                finalScoreText.text = $"Score: {finalScore}";
        }
    }

    /*
    public class FinishLine_GameManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The player character that moves through the level.")]
        public Transform player;

        [Tooltip("The goal or endpoint of the level.")]
        public Transform endPoint;

        [Tooltip("TMP Text element that displays remaining distance.")]
        public TMP_Text distanceText;

        [Tooltip("UI Panel that appears when the player wins.")]
        public GameObject winPanel;

        [Tooltip("TMP Text for displaying the final time.")]
        public TMP_Text finalTimeText;

        [Tooltip("TMP Text for displaying the final score.")]
        public TMP_Text finalScoreText;

        [Header("Game Settings")]
        [Tooltip("Minimum distance from the endpoint to trigger the win.")]
        public float winDistanceThreshold = 1.5f;

        [Tooltip("Maximum score value if the player reaches instantly.")]
        public int maxScore = 1000;

        private bool gameEnded = false;

        private void Start()
        {
            if (player == null || endPoint == null)
            {
                Debug.LogError("GameManager: Missing Player or EndPoint reference!");
                enabled = false;
                return;
            }

            // Hide win panel at start
            if (winPanel != null)
                winPanel.SetActive(false);
        }

        private void Update()
        {
            if (gameEnded) return;

            // Calculate and display distance
            float distance = Vector3.Distance(player.position, endPoint.position);
            if (distanceText != null)
                distanceText.text = $"Distance: {distance:F1}m";

            // Check win condition
            if (distance <= winDistanceThreshold)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            gameEnded = true;

            // Fetch time and bonus data
            float timeTaken = UI_GameTimer.Instance.fn_GetElapsedTime();
            int bonusScore = UI_Score_Singelton.Instance.fn_GetScore();

            // Calculate time-based score (faster = better)
            int timeScore = Mathf.Max(0, Mathf.RoundToInt(maxScore - timeTaken * 50f));

            // Combine scores
            int finalScore = timeScore + bonusScore;

            Debug.Log($"🏁 You win! Time: {timeTaken:F2}s | Time Score: {timeScore} | Bonus: {bonusScore} | Final Score: {finalScore}");

            // Show win panel
            if (winPanel != null)
                winPanel.SetActive(true);

            // Update TMP texts
            if (finalTimeText != null)
                finalTimeText.text = $"Time: {timeTaken:F2}s";

            if (finalScoreText != null)
                finalScoreText.text = $"Score: {finalScore}";
        }
    }
*/
}