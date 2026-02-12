using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExpandableUI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class ExpandableElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Expansion Settings")]
        [SerializeField] private float collapsedHeight = 100f;
        [SerializeField] private float expandedHeight = 200f;
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [Header("References")]
        [SerializeField] private ExpandableUIManager uiManager;
        private RectTransform rectTransform;
        private bool isExpanded = false;
        private Coroutine animationCoroutine;
        [SerializeField] private GameObject recipeContainer;
        [SerializeField] private GameObject drinkTitle;
        [SerializeField] private GameObject drinkDescription;
        private Image image;
    
        private void Awake()
        {
            //Get collider size of recipe box
            rectTransform = GetComponent<RectTransform>();

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, collapsedHeight);

            if (uiManager == null)
            {
                uiManager = FindObjectOfType<ExpandableUIManager>();
            }
        
            image = gameObject.GetComponent<UnityEngine.UI.Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleExpansion();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Hover effect 
            if (image != null)
            {
                image.color = Color.yellow;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Hover effect 
            if (image != null)
            {
                image.color = Color.white;
            }
        }

        public void ToggleExpansion()
        {
            isExpanded = !isExpanded;

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(AnimateHeight(
                isExpanded ? expandedHeight : collapsedHeight
            ));

            if (isExpanded)
                GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.ElementExpanded, gameObject.name);

            StartCoroutine(SwitchText());
        }

        IEnumerator SwitchText()
        {
            yield return new WaitForSeconds(0.1f); 
            // Disable the drink title
            if (isExpanded)
            {
                drinkTitle.SetActive(false);
                drinkDescription.SetActive(true);
            }
            else
            {
                drinkTitle.SetActive(true);
                drinkDescription.SetActive(false);
            }
        }
    
        private IEnumerator AnimateHeight(float targetHeight)
        {
            float startHeight = rectTransform.rect.height;
            float time = 0;

            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float normalizedTime = time / animationDuration;
                float evaluatedTime = animationCurve.Evaluate(normalizedTime);

                float newHeight = Mathf.Lerp(startHeight, targetHeight, evaluatedTime);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

                if (TryGetComponent<BoxCollider2D>(out var collider))
                {
                    collider.size = new Vector2(rectTransform.rect.width, newHeight);
                }

                // Notify the manager that element size changed
                if (uiManager != null)
                {
                    uiManager.ElementSizeChanged(gameObject);
                }

                yield return null;
            }

            // Make sure element stops expanding at exactly the target height
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);

            if (TryGetComponent<BoxCollider2D>(out var finalCollider))
            {
                finalCollider.size = new Vector2(rectTransform.rect.width, targetHeight);
            }

            animationCoroutine = null;
        }
    
        public bool IsExpanded()
        {
            return isExpanded;
        }
    
        public float GetCollapsedHeight()
        {
            return collapsedHeight;
        }
    
        public float GetExpandedHeight()
        {
            return expandedHeight;
        }
    }
}