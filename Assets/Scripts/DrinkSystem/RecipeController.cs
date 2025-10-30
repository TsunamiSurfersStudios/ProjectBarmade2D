using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tier
{
    [SerializeField] Ingredient[] ingredients;
    public Ingredient[] GetIngredients()
    {
        return ingredients;
    }
}
public class RecipeController : MonoBehaviour
{
    [SerializeField] Tier[] tiers;

    private void Start()
    {
        /*
        // Verify that all ingredients are being used. 
        Ingredient[] allIngredients = Resources.FindObjectsOfTypeAll<Ingredient>();
        // Build hash map
        HashSet<Ingredient> ingredients = new HashSet<Ingredient>();
        foreach (Tier tier in tiers)
        {
            foreach (Ingredient ingredient in tier.GetIngredients())
            {
                ingredients.Add(ingredient);
            }
        }
        foreach (Ingredient ingredient in allIngredients)
        {
            if (!ingredients.Contains(ingredient))
            {
                Debug.Log($"[RecipeController] {ingredient.GetName()} is not currently assigned to recipe controller.");
            }
        }
        */
    }

    /// Class should be able to pring all recipes that can be unlocked at a specific tier (put this in tests)
    /// class should track current level and return avaliable ingredients
    /// given an ingredient, return boolean if it is unlocked
    /// unlock new level ==> returns int
    /// given level num, return all recipes that are unlocked at that level
    /// given level num, return all ingredients that are unlocked at that level
    /// given level num, return all unlocked ingredients/recipes
}
