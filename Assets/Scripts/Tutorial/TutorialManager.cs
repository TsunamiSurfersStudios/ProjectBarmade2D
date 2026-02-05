using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using static GameEventManager;

[System.Serializable]
public class TutorialStep
{
    [Header("Trigger Conditions")]
    public TriggerType triggerType;
    public GameEvent gameEvent; 

    [Header("Tooltip")]
    public string tooltipText;
    public Vector2 tooltipPosition;
    public TooltipAnchor anchor = TooltipAnchor.Center;
    public GameObject targetObject; 

    [Header("Progression")]
    public ProgressionType progressionType;
    public GameEvent progressionEventName; 
    public float autoProgressDelay = 0f; 
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

public enum TooltipAnchor
{
    TopLeft, Top, TopRight,
    Left, Center, Right,
    BottomLeft, Bottom, BottomRight,
    FollowTarget // Follow the targetObject
}

/*******************************************************************************************************/
[CreateAssetMenu(fileName = "NewTutorialSequence", menuName = "Tutorial/Tutorial Sequence")]
public class TutorialSequence : ScriptableObject
{
    public List<TutorialStep> steps;
    public bool canSkip = true;
    public bool pauseGame = false; // TODO: Remove this
}

/*******************************************************************************************************/

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    public TutorialTooltipUI tooltipUI;
    public TutorialSequence activeSequence;

    [Header("Settings")]
    public bool startOnAwake = true;

    private int currentStepIndex = -1;
    private TutorialStep currentStep;
    private bool waitingForProgression = false;
    private System.Action currentTriggerHandler;
    private System.Action currentProgressionHandler;

    void Start()
    {
        if (startOnAwake && activeSequence != null)
            StartTutorial(activeSequence);
    }

    public void StartTutorial(TutorialSequence sequence)
    {
        activeSequence = sequence;
        currentStepIndex = -1;

        if (sequence.pauseGame)
            Time.timeScale = 0f;

        AdvanceToNextStep();
    }

    void AdvanceToNextStep()
    {
        // Clean up current step
        if (currentStep != null)
            CleanupCurrentStep();

        currentStepIndex++;

        if (currentStepIndex >= activeSequence.steps.Count)
        {
            EndTutorial();
            return;
        }

        currentStep = activeSequence.steps[currentStepIndex];
        ExecuteStep(currentStep);
    }

    void ExecuteStep(TutorialStep step)
    {
        switch (step.triggerType)
        {
            case TriggerType.Immediate:
                ShowTooltip(step);
                break;

            case TriggerType.WaitForEvent:
                if (step.gameEvent != null)
                {
                    currentTriggerHandler = () => {
                        step.gameEvent.OnRaised -= currentTriggerHandler;
                        currentTriggerHandler = null;
                        ShowTooltip(step);
                    };
                    step.gameEvent.OnRaised += currentTriggerHandler;
                }
                break;

            case TriggerType.Manual:
                break;
        }
    }

    void ShowTooltip(TutorialStep step)
    {
        tooltipUI.Show(step);
        SetupProgression(step);
    }

    void SetupProgression(TutorialStep step)
    {
        waitingForProgression = true;

        switch (step.progressionType)
        {
            case ProgressionType.ClickToContinue:
                tooltipUI.EnableClickToProgress(() => OnStepComplete());
                break;

            case ProgressionType.WaitForEvent:
                if (step.progressionEventName != null)
                {
                    currentProgressionHandler = OnStepComplete;
                    step.progressionEventName.OnRaised += currentProgressionHandler;
                }
                break;

            case ProgressionType.AutoProgress:
                if (step.autoProgressDelay > 0)
                    StartCoroutine(AutoProgressCoroutine(step.autoProgressDelay));
                break;
        }
    }

    IEnumerator AutoProgressCoroutine(float delay)
    {
        if (activeSequence.pauseGame)
            yield return new WaitForSecondsRealtime(delay);
        else
            yield return new WaitForSeconds(delay);

        OnStepComplete();
    }

    void OnStepComplete()
    {
        if (!waitingForProgression) return;
        waitingForProgression = false;

        AdvanceToNextStep();
    }

    void CleanupCurrentStep()
    {
        tooltipUI.Hide();

        if (currentStep != null && currentStep.progressionType == ProgressionType.WaitForEvent && currentStep.progressionEventName != null)
        {
            currentStep.progressionEventName.OnRaised -= currentProgressionHandler;
            currentProgressionHandler = null;
        }
    }

    void EndTutorial()
    {
        tooltipUI.Hide();

        if (activeSequence.pauseGame)
            Time.timeScale = 1f;

        currentStep = null;
        currentStepIndex = -1;
    }

    public void SkipTutorial()
    {
        if (activeSequence != null && activeSequence.canSkip)
            EndTutorial();
    }
}