using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCOrdering : MonoBehaviour
{
    Recipe order;
    public Recipe GetRandomRecipe()
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
    }

    public float GetRecipeAccuracy(Recipe recipe, DrinkController drink)
    {
        float accuracy = 0f;

        return accuracy;
    }
}
