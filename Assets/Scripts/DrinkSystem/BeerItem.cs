using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerItem : DrinkController
{
    public Recipe BeerRecipe;

    public void SayName()
    {
        Debug.Log(gameObject.name);
    }
    
    public void SpawnBeer()
    {
        SpawnBeerDrink(BeerRecipe);
    }
}
