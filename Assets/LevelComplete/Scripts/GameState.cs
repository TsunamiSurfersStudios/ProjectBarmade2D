using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// singleton
public class GameState : MonoBehaviour
{
    private static GameState _instance;
    public static GameState Instance { get { return _instance; } }

    // Level Logic
    public Level currentLevel;
    public static event Action onWinLevel;
    public static event Action onLoseLevel;

    // Recipe Progression
    public HashSet<Recipe> unlockedRecipes { private set; get; }
    public HashSet<Ingredient> unlockedIngredients { private set; get; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        unlockedRecipes = new HashSet<Recipe>();
        unlockedIngredients = new HashSet<Ingredient>();
    }

    public void LoseLevel()
    {
        onLoseLevel.Invoke();
    }

    public void WinLevel()
    {
        foreach (Recipe r in currentLevel.recipeRewards)
        {
            unlockedRecipes.Add(r);
        }
        foreach (Ingredient ing in currentLevel.ingredientRewards)
        {
            unlockedIngredients.Add(ing);
        }
        string allRecipes = string.Join(", ", unlockedRecipes);
        string allIngredients = string.Join(", ", unlockedIngredients);
        Debug.Log($"All Recipes:\n${allRecipes}\nAll Ingredients:\n${allIngredients}\n");
        onWinLevel.Invoke();
    }
}
