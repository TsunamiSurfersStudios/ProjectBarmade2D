using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum IngredientType // TODO: Find a different name :( 
{
    SPIRIT,
    MIXER,
    GARNISH
}

public enum Glass
{
    MARTINI,
    SHOT,
    ROCKS,
    MARGARITA,
    CHAMPANGE,
    WINE,
    BEER,
    COCKTAIL,
    HIGHBALL
}

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Bar/Ingredient")]

[System.Serializable]
public class Ingredient : ScriptableObject
{
    [SerializeField] string ingredientName;
    [SerializeField] IngredientType type;
    [Range(0f, 1f)] [SerializeField] float alcoholPercentage = 0f;
    public static Ingredient Create(string name, IngredientType type, float percentage)
    {
        Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
        ingredient.ingredientName = name;
        ingredient.type = type;
        ingredient.alcoholPercentage = percentage;
        return ingredient;
    }

    public float GetAlcoholPercentage()
    { return alcoholPercentage; }

    public void SetPercentage(float percentage)
    {
        alcoholPercentage = percentage;
    }

    public void SetType(IngredientType type)
    {
        this.type = type;
    }

    public IngredientType GetIngredientType()
    { return type; }

    public string GetName() { return ingredientName; }
}

[System.Serializable]
public class DrinkComponent
{
    [SerializeField] Ingredient ingredient;
    [SerializeField] int milliliters = 0; // Amount of this ingredient in the drink

    public static DrinkComponent Create(Ingredient ingredient, int milliliters)
    {
        DrinkComponent drink = new DrinkComponent();
        drink.ingredient = ingredient;
        drink.milliliters = milliliters;
        return drink;
    }

    public void AddIngredient(Ingredient newIngredient)
    {
        if (ingredient) { GameObject.Destroy(ingredient); }
        ingredient = newIngredient;
    }

    public void AddMilliliters(int amount)
    {
        milliliters += amount;
    }

    public Ingredient GetIngredient() { return ingredient; }
    public int GetMilliliters() { return milliliters; }
    public float GetAlcoholAmount() { return ingredient.GetAlcoholPercentage() * milliliters; }
    public string GetIngredientName() { return ingredient.GetName(); }
}

// TODO: Consider refactoring so DrinkComponent extends ingredient. 