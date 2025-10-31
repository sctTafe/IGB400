using TMPro;
using UnityEngine;

public class UI_GameTimer : Singleton<UI_GameTimer>
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = GetFormattedTime(elapsedTime);
        }
    }

    public string GetFormattedTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    // Optional control methods
    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public float fn_GetElapsedTime()
    {
        return elapsedTime;
    }
}
