using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerItem : DrinkController
{
    [SerializeField] private Ingredient ingredient;
    public void SayName()
    {
        Debug.Log(gameObject.name);
    }
    
    public void SpawnBeer()
    {
        SpawnDrink(ingredient);
    }
}
