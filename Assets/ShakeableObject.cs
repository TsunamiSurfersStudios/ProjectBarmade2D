using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ShakeableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drag Settings")]
    [Tooltip("How smoothly the object follows the cursor (lower = more lag)")]
    [SerializeField] private float followSpeed = 15f;

    [Tooltip("How much the object bobs up and down while dragging")]
    [SerializeField] private float bobIntensity = 10f;

    [Tooltip("How fast the bobbing animation occurs")]
    [SerializeField] private float bobSpeed = 8f;

    [Header("Rotation Settings")]
    [Tooltip("Maximum rotation angle in degrees")]
    [SerializeField] private float maxRotation = 15f;

    [Tooltip("How quickly the rotation responds to movement")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Return Settings")]
    [Tooltip("How quickly the object returns to its original position")]
    [SerializeField] private float returnSpeed = 10f;

    [Tooltip("How quickly rotation returns to zero")]
    [SerializeField] private float rotationReturnSpeed = 8f;

    [Tooltip("Enable to see debug messages")]
    [SerializeField] private bool debugMode = false;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Vector2 targetPosition;
    private Vector2 previousPosition;
    private bool isDragging = false;
    private bool isReturning = false;
    private float bobTimer = 0f;
    private float currentRotation = 0f;
    private float targetRotation = 0f;
    private Coroutine returnCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (rectTransform == null)
        {
            Debug.LogError("ShakeableUIImage: No RectTransform found on this GameObject!");
        }

        if (canvas == null)
        {
            Debug.LogError("ShakeableUIImage: No Canvas found in parent hierarchy!");
        }

        // Store the original anchored position
        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition;
        previousPosition = originalPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        isReturning = false;
        bobTimer = 0f;
        previousPosition = rectTransform.anchoredPosition;

        // Stop any return animation
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }

        if (debugMode)
        {
            Debug.Log("ShakeableUIImage: Started dragging!");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        // Convert screen point to local point in the canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        targetPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (debugMode)
        {
            Debug.Log("ShakeableUIImage: Stopped dragging, returning to original position!");
        }

        // Start return animation
        returnCoroutine = StartCoroutine(ReturnToOriginalPosition());
    }

    private void Update()
    {
        if (isDragging)
        {
            // Smoothly follow the cursor
            Vector2 currentPos = rectTransform.anchoredPosition;
            Vector2 smoothPos = Vector2.Lerp(currentPos, targetPosition, followSpeed * Time.deltaTime);

            // Calculate velocity (direction of movement)
            Vector2 velocity = smoothPos - previousPosition;
            previousPosition = smoothPos;

            // Calculate target rotation based on horizontal velocity
            // Positive velocity (moving right) = negative rotation (tilt right)
            // Negative velocity (moving left) = positive rotation (tilt left)
            targetRotation = -velocity.x * maxRotation;
            targetRotation = Mathf.Clamp(targetRotation, -maxRotation, maxRotation);

            // Smoothly interpolate current rotation to target rotation
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Add bobbing effect
            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobIntensity;

            rectTransform.anchoredPosition = smoothPos + new Vector2(0f, bobOffset);
            rectTransform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
        }
        else if (isReturning)
        {
            // Smoothly return rotation to zero
            currentRotation = Mathf.Lerp(currentRotation, 0f, rotationReturnSpeed * Time.deltaTime);
            rectTransform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        isReturning = true;

        while (Vector2.Distance(rectTransform.anchoredPosition, originalPosition) > 0.1f || Mathf.Abs(currentRotation) > 0.1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                originalPosition,
                returnSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Snap to exact position and rotation
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.rotation = Quaternion.identity;
        currentRotation = 0f;
        targetRotation = 0f;
        isReturning = false;

        if (debugMode)
        {
            Debug.Log("ShakeableUIImage: Returned to original position!");
        }
    }

    private void OnDisable()
    {
        isDragging = false;
        isReturning = false;

        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.rotation = Quaternion.identity;
        }

        currentRotation = 0f;
        targetRotation = 0f;
    }

    private void OnValidate()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (!isDragging && !isReturning && rectTransform != null)
        {
            originalPosition = rectTransform.anchoredPosition;
        }
    }
}