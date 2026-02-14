using Game;
using UnityEngine;

namespace Tutorial
{
    [System.Serializable]
    public class TutorialStep
    {
        [Header("Trigger Conditions")]
        public TriggerType triggerType;
        public GameEventManager.GameEvent gameEvent;

        [Header("Tooltip")]
        [TextArea] public string tooltipText;
        public GameObject targetObject;

        [Header("Progression")]
        public ProgressionType progressionType;
        public EventCondition progressionCondition;

        [Tooltip("Use with WaitForEvent")] public GameEventManager.GameEvent eventToContinue;
        public float autoProgressDelay = 0f;
        public GameEventManager.Command executeOnComplete;
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
}