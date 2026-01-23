using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] List<Recipe> recipeRewards;
    [SerializeField] List<Ingredient> ingredientRewards;

    public void print()
    {
        Debug.Log("You unlocked these recipes:");
        foreach (Recipe r in recipeRewards)
        {
            Debug.Log(r.GetDrinkName());
        }
        Debug.Log("You unlocked these ingredients:");
        foreach(Ingredient ing in ingredientRewards)
        {
            Debug.Log(ing.GetName());
        }
    }
}
