using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDrinkRecipe", menuName = "Bar/DrinkRecipe")]
public class Recipe : ScriptableObject
{
    [Header("Drink Info")]
    [SerializeField] string drinkName;
    [SerializeField] List<DrinkComponent> spirits;
    [SerializeField] List<DrinkComponent> mixers;
    [SerializeField] List<Ingredient> garnishes;
    [SerializeField] Glass glass;
    [SerializeField] bool hasIce;
    [SerializeField] bool isBlended;
    [Header("Sale Info")]
    [SerializeField] float price = 0f;
    [SerializeField] bool isUnlocked;

    public bool getUnlocked()
    { return isUnlocked; }
    public static Recipe Create(string name, List<DrinkComponent> spirits, List<DrinkComponent> mixers, 
        List<Ingredient> garnishes, Glass glass, bool hasIce, bool isBlended)
    {
        Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
        recipe.drinkName = name;
        recipe.spirits = spirits;
        recipe.mixers = mixers;  
        recipe.garnishes = garnishes;
        recipe.glass = glass;
        recipe.hasIce = hasIce;
        recipe.isBlended = isBlended;
        return recipe;
    }

    public float GetAmountEarned(float multiplier)
    {
        return price * multiplier;
    }

    public string GetDrinkName() { return drinkName; }
    public List<DrinkComponent> GetSpirits() { return spirits; }
    public List<DrinkComponent> GetMixers() {  return mixers; }
    public List<Ingredient> GetGarnishes() { return garnishes; }
    public Glass GetGlass() {  return glass; }
    public bool HasIce() { return hasIce; }
}
