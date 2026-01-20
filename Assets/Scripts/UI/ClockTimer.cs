using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ClockTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    void Update()
    {
        if (TimeController.Instance != null && timerText != null)
        {
            timerText.text = TimeController.Instance.GetTimeString();
        }
    }

}