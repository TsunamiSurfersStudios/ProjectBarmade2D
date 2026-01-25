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
    [SerializeField] bool isBlended; // Don't delete field. To be implemented
    [Header("Sale Info")]
    [SerializeField] float price = 0f;
    [SerializeField] Sprite sprite;

    bool isUnlocked = false; // TODO: may deprecate this field because of RecipeContoller 
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
    public bool CanMakeDrink(HashSet<Ingredient> unlockedIngredients)
    {
        bool HasAll<T>(IEnumerable<T> components, System.Func<T, Ingredient> selector)
        {
            if (components == null) return true;
            foreach (var c in components)
            {
                if (!unlockedIngredients.Contains(selector(c)))
                    return false;
            }
            return true;
        }

        return HasAll(spirits, s => s.GetIngredient()) &&
               HasAll(mixers, m => m.GetIngredient()) &&
               HasAll(garnishes, g => g);
    }

    public bool CanMakeDrink(IEnumerable<Ingredient> unlockedIngredients)
    {
        HashSet<Ingredient> unlockedSet = new HashSet<Ingredient>(unlockedIngredients);
        return CanMakeDrink(unlockedSet);
    }

}
