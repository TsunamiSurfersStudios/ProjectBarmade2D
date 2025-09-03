using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bottle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DrinkComponent drinkType;

    public void OnPointerClick(PointerEventData eventData)
    {
        DrinkMixing drinkMakingStation = FindObjectOfType<DrinkMixing>(true);
        drinkMakingStation.GetComponent<DrinkMixing>().SetSelectedDrink(drinkType);
    }
}
