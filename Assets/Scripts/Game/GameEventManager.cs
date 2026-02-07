using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public enum GameEvent
    {
        NONE, // Use this for uninitialized events
        LevelComplete,
        NPCSpawned, 
        CustomerCollided,
        CustomerInteracted,
        CustomerOrdered,
        DrinkMixingStationCollided,
        DrinkMixingStationInteracted,
        ElementExpanded,
        IngredientAdded,
        IceMachineInteracted,
        IceTrayRefilled,
        DrinkCreated,
        CustomerServed,
    }

    public enum Command
    {
        NONE, 
        SpawnNPC,
        StartDay,
        HideUI
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

    private Dictionary<GameEvent, object> events = new Dictionary<GameEvent, object>();
    private Dictionary<Command, object> commands = new Dictionary<Command, object>();

    public void Subscribe(GameEvent eventName, Action callback)
    {
        if (!events.ContainsKey(eventName))
            events[eventName] = null;
        events[eventName] = (Action)events[eventName] + callback;
    }

    public void Subscribe<T>(GameEvent eventName, Action<T> callback)
    {
        if (!events.ContainsKey(eventName))
            events[eventName] = null;
        events[eventName] = (Action<T>)events[eventName] + callback;
    }

    public void Unsubscribe(GameEvent eventName, Action callback)
    {
        if (events.ContainsKey(eventName) && events[eventName] is Action action)
            events[eventName] = action - callback;
    }

    public void Unsubscribe<T>(GameEvent eventName, Action<T> callback)
    {
        if (events.ContainsKey(eventName) && events[eventName] is Action<T> action)
            events[eventName] = action - callback;
    }

    public void TriggerEvent(GameEvent eventName)
    {
        if (events.ContainsKey(eventName) && events[eventName] is Action action)
            action?.Invoke();
    }

    public void TriggerEvent(string eventName)
    {
        if (Enum.TryParse(eventName, out GameEvent gameEvent))
        {
            TriggerEvent(gameEvent);
        }
        else
        {
            Debug.LogWarning($"GameEventManager: No GameEvent found with name {eventName}");
        }
    }

    public void TriggerEvent<T>(GameEvent eventName, T arg)
    {
        if (events.ContainsKey(eventName) && events[eventName] is Action<T> action)
            action?.Invoke(arg);
    }

    public void Subscribe(Command commandName, Action callback)
    {
        if (!commands.ContainsKey(commandName))
            commands[commandName] = null;
        commands[commandName] = (Action)commands[commandName] + callback;
    }

    public void Subscribe<T>(Command commandName, Action<T> callback)
    {
        if (!commands.ContainsKey(commandName))
            commands[commandName] = null;
        commands[commandName] = (Action<T>)commands[commandName] + callback;
    }

    public void Unsubscribe(Command commandName, Action callback)
    {
        if (commands.ContainsKey(commandName) && commands[commandName] is Action action)
            commands[commandName] = action - callback;
    }

    public void Unsubscribe<T>(Command commandName, Action<T> callback)
    {
        if (commands.ContainsKey(commandName) && commands[commandName] is Action<T> action)
            commands[commandName] = action - callback;
    }

    public void TriggerEvent(Command commandName)
    {
        if (commands.ContainsKey(commandName) && commands[commandName] is Action action)
            action?.Invoke();
    }

    public void TriggerEvent<T>(Command commandName, T arg)
    {
        if (commands.ContainsKey(commandName) && commands[commandName] is Action<T> action)
            action?.Invoke(arg);
    }
}