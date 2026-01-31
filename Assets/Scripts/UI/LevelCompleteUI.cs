using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] GameObject ScreenDayComplete;
    [SerializeField] GameEvent EventDayComplete;

    void Awake()
    {
        EventDayComplete.OnRaised += Show;
        ScreenDayComplete.SetActive(false);
    }

    void Show()
    {
        ScreenDayComplete.SetActive(true);
    }
}
