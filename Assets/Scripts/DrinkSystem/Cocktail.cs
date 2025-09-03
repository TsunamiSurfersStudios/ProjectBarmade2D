using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Cocktail : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        DrinkMixing drinkMakingStation = FindObjectOfType<DrinkMixing>(true);
        DrinkComponent selectedDrink = drinkMakingStation.GetComponent<DrinkMixing>().GetSelectedDrink();
        Ingredient selectedGarnish = drinkMakingStation.GetComponent<DrinkMixing>().GetSelectedGarnish();
        bool iceSelected = drinkMakingStation.GetComponent<DrinkMixing>().GetIceSelected();
        if (selectedDrink != null)
        {
            gameObject.GetComponent<DrinkController>().AddIngredient(selectedDrink.GetIngredient(), selectedDrink.GetMilliliters());
            drinkMakingStation.GetComponent<DrinkMixing>().SetSelectedDrink(null);
        }
        if (selectedGarnish != null)//TODO: This needs to deduct the garnish from the storage
        {
            gameObject.GetComponent<DrinkController>().AddGarnish(selectedGarnish);
            drinkMakingStation.GetComponent<DrinkMixing>().SetSelectedGarnish(null);
        }

        if (iceSelected)
        {
            gameObject.GetComponent<DrinkController>().AddIce();
            drinkMakingStation.GetComponent<DrinkMixing>().SetIceSelected(false);
        }
    }
}
