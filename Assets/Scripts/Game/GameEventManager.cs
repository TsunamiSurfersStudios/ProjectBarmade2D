using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public enum GameEvent
    {
        
    }

    private static GameEventManager _instance;
    public static GameEventManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameEventManager>();
            return _instance;
        }
    }

    private Dictionary<GameEvent, Action> events = new Dictionary<GameEvent, Action>();

    public void Subscribe(GameEvent eventName, Action callback)
    {
        if (!events.ContainsKey(eventName))
            events[eventName] = null;

        events[eventName] += callback;
    }

    public void Unsubscribe(GameEvent eventName, Action callback)
    {
        if (events.ContainsKey(eventName))
            events[eventName] -= callback;
    }

    public void TriggerEvent(GameEvent eventName)
    {
        if (events.ContainsKey(eventName))
            events[eventName]?.Invoke();
    }
}