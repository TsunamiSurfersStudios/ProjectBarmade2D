using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TutorialTooltipUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject tooltipPanel;
    [SerializeField] TextMeshProUGUI tooltipText;
    [SerializeField] Button continueButton;
    [SerializeField] Image highlightOverlay;

    private Action onClickCallback;

    void Awake()
    {
        tooltipPanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void Show(TutorialStep step)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = step.tooltipText;

        //PositionTooltip(step);

        if (step.targetObject != null && highlightOverlay != null)
            HighlightTarget(step.targetObject);

        continueButton.gameObject.SetActive(step.progressionType == ProgressionType.ClickToContinue);
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
        if (highlightOverlay != null)
            highlightOverlay.gameObject.SetActive(false);
        onClickCallback = null;
    }

    public void EnableClickToProgress(Action callback)
    {
        onClickCallback = callback;
    }

    void OnContinueClicked()
    {
        onClickCallback?.Invoke();
    }

    //void PositionTooltip(TutorialStep step)
    //{
    //    if (step.anchor == TooltipAnchor.FollowTarget && step.targetObject != null)
    //    {
    //        tooltipRect.position = step.targetObject.transform.position;
    //    }
    //    else
    //    {
    //        tooltipRect.anchoredPosition = step.tooltipPosition;
    //    }
    //}

    void HighlightTarget(GameObject target)
    {
        if (highlightOverlay != null)
            highlightOverlay.gameObject.SetActive(true);
    }
}