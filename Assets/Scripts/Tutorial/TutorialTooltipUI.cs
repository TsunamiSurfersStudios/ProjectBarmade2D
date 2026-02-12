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
    private GameObject currentTarget;
    private string originalSortingLayer;
    private int originalSortingPosition;

    void Awake()
    {
        tooltipPanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void Show(TutorialStep step)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = step.tooltipText;

        if (step.targetObject != null && highlightOverlay != null)
        {
            currentTarget = step.targetObject; // Store the target
            HighlightTarget(step.targetObject);
        }

        continueButton.gameObject.SetActive(step.progressionType == ProgressionType.ClickToContinue);
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);

        ResetCamera();
        if (highlightOverlay != null)
            highlightOverlay.gameObject.SetActive(false);

        if (currentTarget != null)
        {
            SpriteRenderer targetSprite = currentTarget.GetComponent<SpriteRenderer>();
            if (targetSprite)
            {
                targetSprite.sortingLayerName = originalSortingLayer;
                targetSprite.sortingOrder = originalSortingPosition;
            }
        }

        currentTarget = null;
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

    void HighlightTarget(GameObject target)
    {
        FollowTarget(target);
        if (highlightOverlay != null)
        {
            highlightOverlay.gameObject.SetActive(true);
            SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
            if (targetSprite)
            {
                originalSortingLayer = targetSprite.sortingLayerName;
                originalSortingPosition = targetSprite.sortingOrder;
                targetSprite.sortingLayerName = "UI";
                targetSprite.sortingOrder = 100;
            }
        }
    }

    void FollowTarget(GameObject target)
    {
        Camera camera = Camera.main;

        if (camera != null)
        {
            // Get virtual camera from the camera's GameObject or find it in scene
            Cinemachine.CinemachineVirtualCamera vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();

            if (vcam != null && target != null)
            {
                vcam.Follow = target.transform;
                vcam.LookAt = target.transform;
            }
        }
    }

    void ResetCamera()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            FollowTarget(player);
        }
    }
}