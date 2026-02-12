// Event handler component attached to each bottle
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BottleEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Bottle bottle;
    private BottleHoverEffect hoverEffect;

    public void Initialize(Bottle bottle, BottleHoverEffect hoverEffect)
    {
        this.bottle = bottle;
        this.hoverEffect = hoverEffect;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverEffect != null)
        {
            hoverEffect.OnBottleHoverEnter(bottle);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverEffect != null)
        {
            hoverEffect.OnBottleHoverExit(bottle);
        }
    }
}