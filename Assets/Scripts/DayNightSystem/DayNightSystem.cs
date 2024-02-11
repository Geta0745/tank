using UnityEngine;
using TMPro;

public class DayNightSystem : MonoBehaviour
{
    [Header("Day Night System")] public TMP_Text timeText;
    public TMP_Text dayText;
    // Current time in the game cycle
    [SerializeField] protected float currentTime = 0f;
    [SerializeField] protected int dayCount = 1;

    // The maximum value representing the full day-night cycle duration
    [SerializeField] protected float maxFullDay = 600f; // 600 seconds = 10 minutes
    [ReadOnly] public string fullTimeText;

    protected virtual void Update()
    {
        // Increment the current time based on real-world time
        currentTime += Time.deltaTime;

        // If current time exceeds or equals maxFullDay, reset it to 0
        if (currentTime >= maxFullDay)
        {
            dayCount++;
            currentTime = 0f;
        }

        // Calculate the hour
        float hoursPerCycle = 24f; // 24 hours in a day
        float hoursPerSecond = hoursPerCycle / maxFullDay;
        int hour = Mathf.FloorToInt(currentTime * hoursPerSecond) % 12;
        if (hour == 0)
        {
            hour = 12; // 12:00 AM
        }

        // Determine if it's AM or PM
        string amPm = currentTime < maxFullDay / 2f ? "AM" : "PM";
        fullTimeText = hour.ToString("D2") + ":00 " + amPm;
        if (timeText != null)
        {
            // Update the UI text
            timeText.text = fullTimeText;
        }

        if (dayText != null)
        {
            dayText.text = "Day " + dayCount.ToString();
        }
    }

    protected string calTime(float cTime)
    {
        // Calculate the hour
        float hoursPerCycle = 24f; // 24 hours in a day
        float hoursPerSecond = hoursPerCycle / maxFullDay;
        int hour = Mathf.FloorToInt(cTime * hoursPerSecond) % 12;
        if (hour == 0)
        {
            hour = 12; // 12:00 AM
        }

        // Determine if it's AM or PM
        string amPm = cTime < maxFullDay / 2f ? "AM" : "PM";
        return hour.ToString("D2") + ":00 " + amPm;
    }
}
