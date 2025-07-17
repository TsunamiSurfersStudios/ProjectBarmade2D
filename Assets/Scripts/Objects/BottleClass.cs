using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// TODO: How is this being used
public class BottleClass : MonoBehaviour, IPointerClickHandler // TODO: Rename 
{
    public enum DrinkType { Soda, Alcohol } // TODO: This seems like a weird way to store information. We should think about maybe making a DrinkType class? 
    public DrinkType drinkType;

    public enum SodaType {Cola, Sprite, Gingerale }
    public enum AlcoholType {Scotch, Rum, Gin, Moonshine, Vodka, Whiskey }

    [HideInInspector] public SodaType sodaType;
    [HideInInspector] public AlcoholType alcoholType;

    private DrinkType previousDrinkType = (DrinkType)(-1);
    public float alcoholVolume = 0f;

    private bool holdingDrink;

    Camera currCamera; // TODO: Nees to be renamed

    private void OnValidate()
    {
        if (drinkType != previousDrinkType)
        {
            //Reset other enum, if alcohol or soda were selected
            if (drinkType == DrinkType.Soda)
            {
                alcoholType = (AlcoholType)0;
            }
            else
            {
                sodaType = (SodaType)0;
            }

            previousDrinkType = drinkType;
        }
    }
    
    void Start()
    {
        Debug.Log(sodaType);
        Debug.Log(alcoholType);
        currCamera = Camera.main;
    }

    void Update()
    {
        if(holdingDrink) 
        {
            Vector3 cameraPos = currCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        holdingDrink = true;
    }
}
