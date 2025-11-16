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

    public bool VerifyAllIngredientsUsed()
    {
        Ingredient[] allIngredients = Resources.LoadAll<Ingredient>(PATH + "/Ingredients");

        HashSet<Ingredient> hashedIngredients = tiers
            .SelectMany(tier => tier.GetIngredients())
            .ToHashSet();

        bool foundAll = true;

        if (!hashedIngredients.SetEquals(allIngredients))
        {
            // Find what's missing or extra
            var unusedIngredients = allIngredients.Except(hashedIngredients);
            var extraIngredients = hashedIngredients.Except(allIngredients);

            foreach (Ingredient ingredient in unusedIngredients)
            {
                Debug.Log($"[RecipeController] {ingredient.GetName()} is not currently assigned to recipe controller.");
            }

            foreach (Ingredient ingredient in extraIngredients)
            {
                Debug.Log($"[RecipeController] {ingredient.GetName()} is assigned but not in Resources.");
            }

            foundAll = false;
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
