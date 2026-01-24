using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] public String Name;
    [SerializeField] public List<Recipe> recipeRewards = new List<Recipe>();
    [SerializeField] public List<Ingredient> ingredientRewards = new List<Ingredient>(); 

    public override String ToString()
    {
        String retVal = $"Level Name: {Name}\nUnlockable Recipes:\n";
        foreach (Recipe r in recipeRewards)
        {
            retVal += $"{r.GetDrinkName()}\n";
        }
        retVal += "Unlockable Ingredients:\n";
        foreach(Ingredient ing in ingredientRewards)
        {
            retVal += $"{ing.GetName()}\n";
        }
        return retVal;
    }
}
