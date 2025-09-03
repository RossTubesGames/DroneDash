using TMPro; // Correct namespace for TextMeshPro
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 120f; // starting time in seconds
    private float timeRemaining;
    private bool timerIsRunning;

    [Header("UI Reference (Optional)")]
    public TextMeshProUGUI timerText; // Drag a TMP UI text here

    void Start()
    {
        ResetTimer(); // Initializes with startTime
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Debug.Log("Time has run out!");
            }
        }
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
        timerIsRunning = true;
        DisplayTime(timeRemaining); // Refresh UI immediately
        Debug.Log("Timer Reset!");
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(0, timeToDisplay + 1); // Prevents negatives
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
