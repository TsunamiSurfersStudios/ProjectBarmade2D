using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bottle : MonoBehaviour
{
    [SerializeField] private DrinkComponent drinkType;

    public void SelectDrink()
    {
        DrinkMixing drinkMakingStation = FindObjectOfType<DrinkMixing>(true);
        drinkMakingStation.GetComponent<DrinkMixing>().SetSelectedDrink(drinkType);
    }
}
