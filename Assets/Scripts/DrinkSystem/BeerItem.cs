using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerItem : MonoBehaviour
{
    [SerializeField] private Ingredient ingredient;

    public Ingredient GetIngredient()
    {
        return ingredient;
    }
}