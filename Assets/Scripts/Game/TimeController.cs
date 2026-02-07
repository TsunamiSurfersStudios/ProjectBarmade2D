using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    // Singleton instance
    public static TimeController Instance { get; private set; }

    [Header("Time Settings")]
    [SerializeField] private float timeScale = 60f;
    [SerializeField] private Day currentDay = Day.Monday;

    private int currentHour;
    private int currentMinute;

    private bool isRunning = false;
    private float elapsedTime;

    // Events for other scripts to subscribe to
    public event Action<int> OnHourChanged;
    public event Action<int, int> OnTimeChanged;
    public event Action<Day> OnDayChanged;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartTime(int startHour)
    {
        Time.timeScale = 1f;
        isRunning = true;
        elapsedTime = startHour * 60f;
        UpdateDisplayTime();
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        int previousHour = currentHour;

        // Advance time
        elapsedTime += Time.deltaTime * timeScale;

        // Wrap around at 24 hours
        if (elapsedTime >= 1440f) // 24 hours * 60 minutes
        {
            elapsedTime -= 1440f;
            currentDay = (Day)(((int)currentDay + 1) % 7);
            OnDayChanged?.Invoke(currentDay);
        }

        UpdateDisplayTime();

        // Trigger events
        OnTimeChanged?.Invoke(currentHour, currentMinute);

        if (currentHour != previousHour)
        {
            OnHourChanged?.Invoke(currentHour);
        }
    }

    private void UpdateDisplayTime()
    {
        currentHour = Mathf.FloorToInt((elapsedTime / 60) % 24);
        currentMinute = Mathf.FloorToInt(elapsedTime % 60);
    }

    // Public accessors
    public int GetCurrentHour() => currentHour;
    public int GetCurrentMinute() => currentMinute;
    public float GetTimeInMinutes() => elapsedTime;

    public string GetTimeString(bool useMilitaryTime = false)
    {
        if (useMilitaryTime)
        {
            return $"{currentHour:D2}:{currentMinute:D2}";
        }
        else
        {
            int displayHour = currentHour % 12;
            if (displayHour == 0) displayHour = 12;
            string period = currentHour < 12 ? "AM" : "PM";
            return $"{displayHour}:{currentMinute:D2} {period}";
        }
    }

    // Utility methods
    public void SetTime(int hour, int minute)
    {
        elapsedTime = hour * 60f + minute;
        UpdateDisplayTime();
    }

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
    }

    public bool IsTimeBetween(int startHour, int endHour)
    {
        if (endHour < startHour)
        {
            // Handles wrap-around (e.g., 22:00 to 06:00)
            return currentHour >= startHour || currentHour < endHour;
        }
        else
        {
            return currentHour >= startHour && currentHour < endHour;
        }
    }

    public bool IsNight()
    {
        return IsTimeBetween(18, 6); // 6 PM to 6 AM
    }

    public bool IsDay()
    {
        return !IsNight();
    }

    public Day DayOfWeek()
    {
        return currentDay;
    }
}
