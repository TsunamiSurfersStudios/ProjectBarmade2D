using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// Referenced from Ryan Hipple: Game Architecture with Scriptable Objects 
// Observable pattern
[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/GameEvent")]
public class GameEvent : ScriptableObject
{
    [HideInInspector]
    public bool hasBeenTriggered { get; private set; }
    private List<GameEventListener> listeners = new List<GameEventListener>();
    public bool TriggerOnceOnly;

    [Tooltip("Game Events that must be triggered before this one")]
    public List<GameEvent> PreRequisites;

    private void OnEnable()
    {
        hasBeenTriggered = false;
    }

    public void Raise()
    {
        //Debug.Log($"has been trigg: {hasBeenTriggered}, trigg once: {TriggerOnceOnly}.");
        if (!checkPrereqs()) return;
        if (hasBeenTriggered && TriggerOnceOnly) return;
        if (!hasBeenTriggered) hasBeenTriggered = true;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    private bool checkPrereqs()
    {
        foreach (GameEvent e in PreRequisites)
        {
            Debug.Log($"{name} -- Checking prereq: {e.name} | Triggered: {e.hasBeenTriggered}");
            if (e == null || !e.hasBeenTriggered)
            {
                return false;
            }
        }
        return true;
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}