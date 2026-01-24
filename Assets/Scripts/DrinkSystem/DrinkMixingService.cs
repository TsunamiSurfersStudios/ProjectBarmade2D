using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkMixingService : MonoBehaviour
{
    private DrinkController drinkController;
    [SerializeField] GameObject starterDrink; // TODO: Use different glass types instead
    public void StartNewDrink()
    {
        if (!starterDrink)
        {
            Debug.LogError("No starter drink prefab assigned to DrinkMixingService.");
            return;
        }

        GameObject newDrink = Instantiate(starterDrink);
        newDrink.AddComponent<DrinkController>();
        drinkController = newDrink.GetComponent<DrinkController>();

        if (drinkController == null)
        {
            Debug.LogError("Failed to add DrinkController to the new drink instance.");
        }
    }


    public void AddIngredient(Ingredient ingredient, int amount = 0)
    {
        drinkController.AddIngredient(ingredient, amount);
    }

    public void SelectGlass(Glass glass)
    {
        drinkController.SetGlass(glass);
    }

    public DrinkController GetDrink()
    {
        return drinkController;
    }

    public void AddIce()
    {
        drinkController.AddIce();
    }

    public void FinishDrink()
    {
        drinkController.Spawn();
        drinkController = null;
    }
}
