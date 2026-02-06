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
    private GameObject currentTarget; // Store current target reference

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

        if (highlightOverlay != null)
            highlightOverlay.gameObject.SetActive(false);

        // Clean up Canvas component from target
        if (currentTarget != null)
        {
            Canvas targetCanvas = currentTarget.GetComponent<Canvas>();
            if (targetCanvas != null)
            {
                Destroy(targetCanvas);
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
        ResetCamera();
        
        // Clean up Canvas from current target
        if (currentTarget != null)
        {
            Canvas targetCanvas = currentTarget.GetComponent<Canvas>();
            if (targetCanvas != null)
            {
                Destroy(targetCanvas);
            }
        }
        onClickCallback?.Invoke();
    }

    void HighlightTarget(GameObject target)
    {
        FollowTarget(target);
        if (highlightOverlay != null)
        {

            //Canvas targetCanvas = target.GetComponent<Canvas>();
            //if (targetCanvas == null)
            //{
            //    targetCanvas = target.AddComponent<Canvas>();
            //    targetCanvas.overrideSorting = true;
            //}

            //Canvas overlayCanvas = highlightOverlay.GetComponentInParent<Canvas>();
            //targetCanvas.sortingOrder = (overlayCanvas?.sortingOrder ?? 0) + 1;
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