using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class Bottle
    {
        public string NAME;
        public string PATH;

        [System.NonSerialized]
        public GameObject gameObject;
        [System.NonSerialized]
        public Image image;
        [System.NonSerialized]
        public RectTransform rectTransform;
        [System.NonSerialized]
        public Canvas canvas;
        [System.NonSerialized]
        public BottleEventHandler eventHandler;

        private int originalSortingOrder;

        public void Initialize(GameObject obj, int sortingOrder, Transform parent, BottleHoverEffect hoverEffect)
        {
            gameObject = obj;
            gameObject.transform.SetParent(parent, false);

            rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null)
                rectTransform = gameObject.AddComponent<RectTransform>();

            image = gameObject.GetComponent<Image>();
            if (image == null)
                image = gameObject.AddComponent<Image>();

            // Load sprite from Resources folder
            Sprite sprite = Resources.Load<Sprite>(PATH);
            if (sprite != null)
            {
                image.sprite = sprite;
                image.preserveAspect = false;
            }
            else
            {
                Debug.LogWarning($"Failed to load sprite at path: {PATH}");
            }

            image.raycastTarget = true;
            // Add Canvas for sorting control
            canvas = gameObject.GetComponent<Canvas>();
            if (canvas == null)
                canvas = gameObject.AddComponent<Canvas>();

            gameObject.AddComponent<GraphicRaycaster>();

            canvas.overrideSorting = true;

            // Add event handler for hover detection
            eventHandler = gameObject.GetComponent<BottleEventHandler>();
            if (eventHandler == null)
                eventHandler = gameObject.AddComponent<BottleEventHandler>();

            eventHandler.Initialize(this, hoverEffect);

            originalSortingOrder = sortingOrder;
            canvas.sortingOrder = sortingOrder;
        }

        public void SetSortingOrder(int order)
        {
            if (canvas != null)
                canvas.sortingOrder = order;
        }

        public int GetOriginalSortingOrder()
        {
            return originalSortingOrder;
        }

        public float GetWidth()
        {
            if (rectTransform != null)
            {
                return rectTransform.rect.width;
            }
            return 0f;
        }
    }



    public class BottleHoverEffect : MonoBehaviour
    {
        [SerializeField] List<Bottle> bottles;
        [SerializeField] GameObject panel;
        [SerializeField] TextMeshProUGUI txtName;

        private Bottle currentHoveredBottle;
        private int maxSortingOrder;

        void Start()
        {
            maxSortingOrder = bottles.Count;
            InitializeBottles();
        }

        void InitializeBottles()
        {
            if (bottles == null || bottles.Count == 0)
            {
                Debug.LogWarning("No bottles to initialize!");
                return;
            }

            if (panel == null)
            {
                Debug.LogError("Panel is not assigned!");
                return;
            }

            RectTransform panelRect = panel.GetComponent<RectTransform>();
            if (panelRect == null)
            {
                Debug.LogError("Panel does not have a RectTransform component!");
                return;
            }

            // Get panel dimensions
            float panelWidth = panelRect.rect.width;
            float panelHeight = panelRect.rect.height;

            // Calculate spacing
            float spacing = panelWidth / bottles.Count; // Default spacing
            float startX = spacing / 2f;

            // Create and position bottles
            for (int i = 0; i < bottles.Count; i++)
            {
                Bottle currBottle = bottles[i];
            
                // Create GameObject for this bottle
                GameObject bottleObj = new GameObject($"Bottle_{bottles[i].NAME}");

                // Initialize with sorting order
                int sortingOrder = CalculateCascadeSortingOrder(i, -1);
                currBottle.Initialize(bottleObj, sortingOrder, panel.transform, this);

                // Scale bottle to target height

                currBottle.rectTransform.sizeDelta = new Vector2(panelHeight, panelHeight);
                currBottle.rectTransform.anchoredPosition = new Vector2(startX +(spacing*i), 0f);
                currBottle.rectTransform.anchorMin = new Vector2(0f, 0.5f);
                currBottle.rectTransform.anchorMax = new Vector2(0f, 0.5f);
                currBottle.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }
        public void OnBottleHoverEnter(Bottle bottle)
        {
            currentHoveredBottle = bottle;

            if (currentHoveredBottle != null)
            {
                // Display name
                if (txtName != null)
                    txtName.text = currentHoveredBottle.NAME;

                bottle.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1f);

                // Update sorting orders for cascade effect
                UpdateCascadeSorting(bottles.IndexOf(currentHoveredBottle));
            }
        }

        // Called by BottleEventHandler when mouse exits a bottle
        public void OnBottleHoverExit(Bottle bottle)
        {
            // Only clear if we're exiting the current hovered bottle
            if (currentHoveredBottle == bottle)
            {
                currentHoveredBottle = null;

                // Clear name when not hovering
                if (txtName != null)
                    txtName.text = "";

                bottle.rectTransform.localScale = new Vector3(1f, 1f, 1f);

                // Reset to default cascade from center
                UpdateCascadeSorting(-1);
            }
        }

        void UpdateCascadeSorting(int hoveredIndex)
        {
            for (int i = 0; i < bottles.Count; i++)
            {
                if (bottles[i] != null)
                {
                    int sortingOrder = CalculateCascadeSortingOrder(i, hoveredIndex);
                    bottles[i].SetSortingOrder(sortingOrder);
                }
            }
        }

        int CalculateCascadeSortingOrder(int bottleIndex, int hoveredIndex)
        {
            if (hoveredIndex == -1)
            {
                // Default: cascade from center outward
                int centerIndex = bottles.Count / 2;
                int distanceFromCenter = Mathf.Abs(bottleIndex - centerIndex);
                return maxSortingOrder - distanceFromCenter;
            }
            else
            {
                // Hovered bottle is on top
                if (bottleIndex == hoveredIndex)
                    return maxSortingOrder + 1;

                // Others cascade based on distance from hovered
                int distance = Mathf.Abs(bottleIndex - hoveredIndex);
                return maxSortingOrder - distance;
            }
        }
    }
}