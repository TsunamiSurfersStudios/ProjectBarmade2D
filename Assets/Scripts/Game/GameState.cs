using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int currentTier { get; private set; }
    public GameEvent EventDayComplete;
    private static GameState _instance;
    public static GameState Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        currentTier = 0; // start at 0
        if(EventDayComplete != null)
        {
            EventDayComplete.OnRaised += incrementTier;
        }
        else { Debug.Log("Day Complete event not set in Game State. "); }
    }

    public void incrementTier()
    {
        currentTier += 1;
        Debug.Log($"current tier: {currentTier}");
    }
}
