using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ClockTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timeMultiplier = 60f; // This will make time pass 60x faster than real time
    float elapsedTime = 0;

    void Update()
    {   
        timerText.text = CalculateTime();
    }

    string CalculateTime()
    {
        // Accumulate time at accelerated rate
        elapsedTime += Time.deltaTime * timeMultiplier;

        int hours = Mathf.FloorToInt(elapsedTime / 3600) % 24; // Convert to hours (0-23)
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60); // Get minutes (0-59)

        // Determine AM or PM
        string period = hours >= 12 ? "AM" : "PM";

        // Convert to 12-hour format
        int displayHour = hours % 12;
        if (displayHour == 0) displayHour = 12; // Display 12 instead of 0 for 12-hour clock format

        return string.Format("{0:00}:{1:00} {2}", displayHour, minutes, period);
    }
}