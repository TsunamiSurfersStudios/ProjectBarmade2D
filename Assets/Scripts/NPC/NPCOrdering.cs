using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEditor;
using UnityEngine;

public class NPCOrdering : MonoBehaviour
{
    Recipe order;
    bool orderActive = false;
    private Recipe GetRandomRecipe()
    {
        // Get unlocked recipes
        List<Recipe> unlockedRecipes = new List<Recipe>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(Recipe)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            Recipe recipe = AssetDatabase.LoadAssetAtPath<Recipe>(assetPath);
            if (recipe != null && recipe.getUnlocked())
            {
                unlockedRecipes.Add(recipe);
            }
        }

        // Get random recipe
        int index = Random.Range(0, unlockedRecipes.Count);
        return unlockedRecipes[index];
    }

    public void CreateOrder()
    {
        order = GetRandomRecipe();
        orderActive = true;
    }

    public Recipe GetOrder() { return order; }

    public float GetRecipeAccuracy(Recipe recipe, DrinkController drink)
    {
        float accuracy = 0f;
        List<DrinkComponent> recipeSpirits = recipe.GetSpirits();
        List<DrinkComponent> recipeMixers = recipe.GetMixers();
        List<DrinkComponent> drinkSpirits = drink.GetSpirits();
        List<DrinkComponent> drinkMixers = drink.GetMixers();

        if (drinkSpirits.Count == 0 && drinkMixers.Count == 0) { return 0; }

        int totalLiquidsExpected = recipeSpirits.Count + recipeMixers.Count;
        int ingredientsFound = 0;
        
        // Check spirits and mixers
        foreach (DrinkComponent spirit in recipeSpirits) // This O^N^2 solution needs to be refactored :(
        {
            foreach(DrinkComponent spirit2 in drinkSpirits) 
            {
                if (spirit.GetIngredientName() == spirit2.GetIngredientName())
                {
                    accuracy += 0.4f / totalLiquidsExpected;
                    if (spirit.GetMilliliters() == spirit2.GetMilliliters()) { accuracy += 0.3f / totalLiquidsExpected; }
                    ingredientsFound++;
                    continue;
                } 
            }
        }
        foreach (DrinkComponent mixer in recipeMixers) // This O^N^2 solution needs to be refactored :(
        {
            foreach (DrinkComponent mixer2 in drinkMixers)
            {
                if (mixer.GetIngredientName() == mixer2.GetIngredientName())
                {
                    accuracy += 0.4f / totalLiquidsExpected;
                    if (mixer.GetMilliliters() == mixer2.GetMilliliters()) { accuracy += 0.3f / totalLiquidsExpected; }
                    ingredientsFound++;
                    continue;
                }
            }
        }
        // Account for wrong ingredients
        accuracy -= (drinkSpirits.Count + drinkMixers.Count - ingredientsFound) * 0.15f;

        // Garnishes
        List < Ingredient > recipeGarnishes = recipe.GetGarnishes();
        List<Ingredient> drinkGarnishes = drink.GetGarnishes();
        int expectedGarnishes = recipeGarnishes.Count;
        int garnishesFound = 0;
        foreach (Ingredient garnish in recipeGarnishes) 
        {
            foreach (Ingredient garnish2 in drinkGarnishes)
            {
                if (garnish.GetName() == garnish2.GetName())
                {
                    accuracy += 0.1f / expectedGarnishes;
                    garnishesFound++;
                    continue;
                }
            }
        }
        accuracy -= (drinkGarnishes.Count - garnishesFound) * 0.05f;
        if (recipe.HasIce() == drink.HasIce()) { accuracy += 0.1f; }
        if (recipe.GetGlass() == drink.GetGlass()) { accuracy += 0.1f; }
        return accuracy;
    }

    public bool OrderActive() { return orderActive; }
}