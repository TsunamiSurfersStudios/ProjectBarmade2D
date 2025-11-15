using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Tier
{
    [SerializeField] List<Ingredient> ingredients;
    public List<Ingredient> GetIngredients()
    {
        return ingredients;
    }
}
public class RecipeController : MonoBehaviour
{
    [SerializeField] Tier[] tiers;
    [SerializeField] string PATH = "Bar"; // For testing purposes only

    HashSet<Ingredient> hashedIngredients;

    public bool VerifyAllIngredientsUsed()
    {
        // Verify that all ingredients are being used. 
        Ingredient[] allIngredients = Resources.LoadAll(PATH + "/Ingredients", typeof(Ingredient)).Cast<Ingredient>().ToArray();
        // Build hash map
        hashedIngredients = new HashSet<Ingredient>();
        foreach (Tier tier in tiers)
        {
            foreach (Ingredient ingredient in tier.GetIngredients())
            {
                hashedIngredients.Add(ingredient);
            }
        }
        bool foundAll = true;
        foreach (Ingredient ingredient in allIngredients)
        {
            if (!hashedIngredients.Contains(ingredient))
            {
                Debug.Log($"[RecipeController] {ingredient.GetName()} is not currently assigned to recipe controller.");
                foundAll = false;
            }
        }
        return foundAll;
    }

    public List<Ingredient> GetIngredientsUnlockedAtLevel(int level)
    {
        if (tiers.Length >= level)
        {
            return tiers[level].GetIngredients();
        }
        return null;
    }
    public IEnumerable<Ingredient> GetAllUnlockedIngredients(int level)
    {
        if (tiers.Length >= level)
        {
            foreach (Ingredient i in tiers[level].GetIngredients())
                yield return i;
        }
        if (level > 0)
        {
            foreach (Ingredient ingredient in GetAllUnlockedIngredients(level - 1))
                yield return ingredient;
        }
    }
    public IEnumerable<Recipe> GetAllUnlockedRecipes(int level)
    {
        Recipe[] allRecipes = Resources.LoadAll(PATH + "/Recipes", typeof(Recipe)).Cast<Recipe>().ToArray();

        foreach (Recipe recipe in allRecipes)
        {
            if (recipe.CanMakeDrink(GetAllUnlockedIngredients(level)))
            {
                yield return recipe;
            }
        }
    }

    /// Class should be able to pring all recipes that can be unlocked at a specific tier (put this in tests)
    /// class should track current level and return avaliable ingredients
    /// given an ingredient, return boolean if it is unlocked
    /// unlock new level ==> returns int
    /// given level num, return all recipes that are unlocked at that level
    /// given level num, return all ingredients that are unlocked at that level
    /// given level num, return all unlocked ingredients/recipes
    /// Cannot duplicate ingredients
}
