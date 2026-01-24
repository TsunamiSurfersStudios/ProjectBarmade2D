using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkMixingService : MonoBehaviour
{
    private DrinkController drink;
    public void StartNewDrink()
    {
        drink = new DrinkController();
    }

    public void AddIngredient(Ingredient ingredient, int amount = 0)
    {
        drink.AddIngredient(ingredient, amount);
    }

    public void SelectGlass(Glass glass)
    {
        drink.SetGlass(glass);
    }

    public DrinkController GetDrink()
    {
        return drink;
    }

    public void AddIce()
    {
        drink.AddIce();
    }

    public void FinishDrink()
    {
        drink.Spawn();
        drink = null; // Reset for next drink
    }
}
