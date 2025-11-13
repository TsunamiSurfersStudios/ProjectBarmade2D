using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Garnish : MonoBehaviour
{
    [SerializeField] private Ingredient ingredientType;

    public void SelectGarnish()
    {
        DrinkMixing drinkMakingStation = FindObjectOfType<DrinkMixing>(true);
        drinkMakingStation.GetComponent<DrinkMixing>().SetSelectedGarnish(ingredientType);
    }
}
