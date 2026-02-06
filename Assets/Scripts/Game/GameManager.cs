using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentTier { get; private set; }
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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
        GameEventManager.Instance.Subscribe(GameEventManager.GameEvent.LevelComplete, incrementTier);
    }

    public void incrementTier()
    {
        currentTier += 1;
        Debug.Log($"current tier: {currentTier}");
    }
}
