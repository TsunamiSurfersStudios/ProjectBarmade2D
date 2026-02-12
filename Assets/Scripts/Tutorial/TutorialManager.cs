using System;
using System.Collections;
using Game;
using UnityEngine;

namespace Tutorial
{
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
        private bool hasSkipped = false;

        void Start()
        {
            if (startOnAwake && activeSequence != null)
                StartTutorial(activeSequence);
        }

        public void StartTutorial(TutorialSequence sequence)
        {
            activeSequence = sequence;
            currentStepIndex = -1;

            tooltipUI.EnableSkip(SkipTutorial);

            AdvanceToNextStep(currentStepIndex + 1);
        }

        void AdvanceToNextStep(int nextStepIndex)
        {
            // Clean up current step
            if (currentStep != null)
                CleanupCurrentStep();

            currentStepIndex = nextStepIndex;
            if (!hasSkipped && currentStepIndex >= activeSequence.stepToSkipTo)
            {
                hasSkipped = true;
            }

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
            ResolvePrefabToInstance(step);

            switch (step.triggerType)
            {
                case TriggerType.Immediate:
                    ShowTooltip(step);
                    break;

                case TriggerType.WaitForEvent:
                    if (step.gameEvent == GameEventManager.GameEvent.NPCSpawned)
                    {
                        Action<GameObject> callback = null;
                        callback = (GameObject obj) =>
                        {
                            step.targetObject = obj;
                            GameEventManager.Instance.Unsubscribe(step.gameEvent, callback);
                            ShowTooltip(step);
                        };
                        GameEventManager.Instance.Subscribe(step.gameEvent, callback);
                    }
                    else
                    {
                        Action callback = null;
                        callback = () =>
                        {
                            GameEventManager.Instance.Unsubscribe(step.gameEvent, callback);
                            ShowTooltip(step);
                        };
                        GameEventManager.Instance.Subscribe(step.gameEvent, callback);
                    }
                    break;

                case TriggerType.Manual:
                    break;
            }
        }

        void ResolvePrefabToInstance(TutorialStep step)
        {
            if (step.targetObject == null)
                return;

            // Check if the target object is a prefab (not in scene)
            if (!step.targetObject.scene.IsValid())
            {
                // It's a prefab, find an instance in the scene
                GameObject instance = FindInstanceOfPrefab(step.targetObject);

                if (instance != null)
                {
                    step.targetObject = instance;
                    Debug.Log($"Tutorial: Resolved prefab to scene instance: {instance.name}");
                }
                else
                {
                    Debug.LogWarning($"Tutorial: Could not find instance of prefab '{step.targetObject.name}' in scene");
                }
            }
        }

        GameObject FindInstanceOfPrefab(GameObject prefab)
        {
            // Get the prefab's name
            string prefabName = prefab.name;

            // Find all objects with the same name
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Check if this object's name matches (Unity removes "(Clone)" suffix automatically in obj.name comparison)
                if (obj.name.Replace("(Clone)", "").Trim() == prefabName)
                {
                    return obj;
                }
            }

            return null;
        }

        void ShowTooltip(TutorialStep step)
        {
            tooltipUI.Show(step, !hasSkipped);
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
                    if (step.progressionCondition != null && step.progressionCondition.conditionType != EventCondition.ConditionType.None)
                    {
                        SubscribeWithCondition(step);
                    }
                    else
                    {
                        Action callback = null;
                        callback = () =>
                        {
                            GameEventManager.Instance.Unsubscribe(step.eventToContinue, callback);
                            OnStepComplete();
                        };
                        GameEventManager.Instance.Subscribe(step.eventToContinue, callback);
                    }
                    break;

                case ProgressionType.AutoProgress:
                    if (step.autoProgressDelay > 0)
                        StartCoroutine(AutoProgressCoroutine(step.autoProgressDelay));
                    break;
            }
        }

        void SubscribeWithCondition(TutorialStep step)
        {
            switch (step.progressionCondition.conditionType)
            {
                case EventCondition.ConditionType.IntEquals:
                    SubscribeWithConditionGeneric<int>(step);
                    break;

                case EventCondition.ConditionType.FloatEquals:
                    SubscribeWithConditionGeneric<float>(step);
                    break;

                case EventCondition.ConditionType.StringEquals:
                    SubscribeWithConditionGeneric<string>(step);
                    break;

                case EventCondition.ConditionType.BoolEquals:
                    SubscribeWithConditionGeneric<bool>(step);
                    break;

                case EventCondition.ConditionType.GameObjectEquals:
                    SubscribeWithConditionGeneric<GameObject>(step);
                    break;

                default:
                    // Fallback to no condition
                    GameEventManager.Instance.Subscribe(step.eventToContinue, OnStepComplete);
                    break;
            }
        }

        void SubscribeWithConditionGeneric<T>(TutorialStep step)
        {
            Action<T> conditionalHandler = null;
            conditionalHandler = (value) =>
            {
                if (step.progressionCondition.Evaluate(value))
                {
                    GameEventManager.Instance.Unsubscribe(step.eventToContinue, conditionalHandler);
                    OnStepComplete();
                }
            };

            GameEventManager.Instance.Subscribe(step.eventToContinue, conditionalHandler);
        }

        IEnumerator AutoProgressCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            OnStepComplete();
        }

        void OnStepComplete()
        {
            if (!waitingForProgression) return;
            waitingForProgression = false;

            GameEventManager.Command command = currentStep.executeOnComplete;
            AdvanceToNextStep(currentStepIndex + 1);
            if (command != GameEventManager.Command.NONE)
            {
                GameEventManager.Instance.TriggerEvent(command);
            }
        }

        void CleanupCurrentStep()
        {
            tooltipUI.Hide();

            if (currentStep != null && currentStep.progressionType == ProgressionType.WaitForEvent)
                GameEventManager.Instance.Unsubscribe(currentStep.eventToContinue, OnStepComplete);
        }

        void EndTutorial()
        {
            tooltipUI.Hide();

            currentStep = null;
            currentStepIndex = -1;
        }

        public void SkipTutorial()
        {
            if (activeSequence != null && activeSequence.canSkip)
            {
                hasSkipped = true;
                AdvanceToNextStep(activeSequence.stepToSkipTo);
            }
        }
    }
}