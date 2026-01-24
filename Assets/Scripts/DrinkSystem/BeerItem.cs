using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerItem : DrinkController
{
    [SerializeField] private Recipe BeerRecipe;
    [SerializeField] private Ingredient ingredient;
    public void SayName()
    {
        Debug.Log(gameObject.name);
    }
    
    public void SpawnBeer()
    {
        SpawnBeerDrink(BeerRecipe);
    }
}
