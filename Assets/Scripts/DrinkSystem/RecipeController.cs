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
    //[SerializeField] GameEvent 

    public bool VerifyRecipeControllerSetup()
    {
        Ingredient[] allIngredients = Resources.LoadAll<Ingredient>(PATH + "/Ingredients");

        HashSet<Ingredient> hashedIngredients = new HashSet<Ingredient>();
        bool foundAll = true;

        // Build hash set and detect duplicates in one pass
        foreach (Tier tier in tiers)
        {
            foreach (Ingredient ingredient in tier.GetIngredients())
            {
                // HashSet.Add returns false if item already exists
                if (!hashedIngredients.Add(ingredient))
                {
                    Debug.Log($"[RecipeController] Duplicate ingredient '{ingredient.GetName()}' found.");
                    foundAll = false;
                }
            }
        }

        // Check for unused ingredients
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
        //TODO: Can we run these operations more efficiently by caching results?
        if (tiers.Length > level)
        {
            foreach (Ingredient i in tiers[level].GetIngredients())
                yield return i;

            if (level > 0)
            {
                foreach (Ingredient ingredient in GetAllUnlockedIngredients(level - 1))
                    yield return ingredient;
            }
        }
    }
    public IEnumerable<Recipe> GetAllUnlockedRecipes(int level)
    {
        //TODO: Can we run these operations more efficiently by caching results?
        Recipe[] allRecipes = Resources.LoadAll(PATH + "/Recipes", typeof(Recipe)).Cast<Recipe>().ToArray();

        foreach (Recipe recipe in allRecipes)
        {
            if (recipe.CanMakeDrink(GetAllUnlockedIngredients(level)))
            {
                yield return recipe;
            }
        }
    }

    public IEnumerable<Recipe> GetNewlyUnlockedRecipes(int level)
    {
        IEnumerable<Recipe> oldRecipes = level > 0 ? GetAllUnlockedRecipes(level-1) : new List<Recipe>();
        IEnumerable<Recipe> newRecipes = GetAllUnlockedRecipes(level);
        return newRecipes.Except(oldRecipes);
    }

    public bool IngredientIsUnlocked(Ingredient ingredient, int level)
    {
        IEnumerable<Ingredient> unlockedIngredients = GetAllUnlockedIngredients(level);
        return unlockedIngredients.Contains(ingredient);
    }
}
