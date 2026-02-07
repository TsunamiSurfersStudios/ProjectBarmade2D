using static GameEventManager;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    [Header("Trigger Conditions")]
    public TriggerType triggerType;
    public GameEvent gameEvent;

    [Header("Tooltip")]
    [TextArea] public string tooltipText;
    public GameObject targetObject;

    [Header("Progression")]
    public ProgressionType progressionType;
    [Tooltip("Use with WaitForEvent")] public GameEvent eventToContinue;
    public float autoProgressDelay = 0f;
    public Command executeOnComplete;
}

public enum TriggerType
{
    Immediate,      // Starts right away
    WaitForEvent,   // Waits for a game event
    Manual          // Triggered by code
}

public enum ProgressionType
{
    ClickToContinue,    // Click anywhere or on tooltip
    WaitForEvent,       // Wait for specific game event
    AutoProgress        // Time-based
}

/*******************************************************************************************************/
[CreateAssetMenu(fileName = "NewTutorialSequence", menuName = "Tutorial/Tutorial Sequence")]
public class TutorialSequence : ScriptableObject
{
    public List<TutorialStep> steps;
    public bool canSkip = true;
    public bool pauseGame = false; // TODO: Remove this
}
