using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandableUIManager : MonoBehaviour
{
    [Tooltip("List of all UI elements that can be expanded/collapsed")]
    [SerializeField] public List<GameObject> expandableElements = new List<GameObject>();

    [Tooltip("Distance between UI elements when not expanded")]
    [SerializeField] private float defaultSpacing = 10f;

    [Tooltip("Spacing buffer when elements expand")]
    [SerializeField] private float expansionBuffer = 5f;
    [SerializeField] private float firstElementY;

    private Dictionary<GameObject, RectTransform> elementRects = new Dictionary<GameObject, RectTransform>();

    //!Potential expandable elements fix idea - Change position based on the elements in the list. If first element expands, all following elements should shift. If middle element expands, top and bottom shifts
    private void Awake()
    {
        foreach (GameObject element in expandableElements)
        {
            elementRects[element] = element.GetComponent<RectTransform>();

            if (!element.TryGetComponent<BoxCollider2D>(out var collider))
            {
                collider = element.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }

            UpdateColliderSize(element);
        }

        ArrangeElements();
    }

    public void ArrangeElements()
    {
        float currentY = firstElementY;

        foreach (GameObject element in expandableElements)
        {
            RectTransform rectTransform = elementRects[element];

            Vector2 position = rectTransform.anchoredPosition;
            position.y = currentY;
            rectTransform.anchoredPosition = position;

            currentY -= (rectTransform.rect.height + defaultSpacing);
        }
    }

    private void UpdateColliderSize(GameObject element)
    {
        if (element.TryGetComponent<BoxCollider2D>(out var collider) &&
            element.TryGetComponent<RectTransform>(out var rectTransform))
        {
            collider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

            collider.offset = new Vector2(0, 0);
        }
    }

    // Call whenever an element's size changes
    public void ElementSizeChanged(GameObject element)
    {
        if (expandableElements.Contains(element))
        {
            UpdateColliderSize(element);
        }
    }
}