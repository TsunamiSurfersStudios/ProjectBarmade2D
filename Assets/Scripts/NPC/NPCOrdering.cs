using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCOrdering : MonoBehaviour
{
    public static event System.Action<string, string> OnOrderCreated;
    public static event System.Action<string> OnOrderCompleted;
    private string customerName;
    public Recipe order { get; private set; }
    bool orderActive = false;

    [SerializeField] private int minDrinks = 1;
    [SerializeField] private int maxDrinks = 3;
    private int drinkLimit;
    private int drinksServed = 0;
    private float tab = 0f;
    private bool wantsToOrderAgain = false;

    void Awake()
    {
        drinkLimit = Random.Range(minDrinks, maxDrinks + 1);
    }

    private Recipe GetRandomRecipe()
    {
        int currentLevel = GameManager.Instance?.currentLevel ?? 0;
        ;
        var unlockedRecipes = RecipeController.Instance.GetAllUnlockedRecipes(currentLevel).ToList();
        if (unlockedRecipes.Count > 0)
        {
            return unlockedRecipes[Random.Range(0, unlockedRecipes.Count)];
        }
        return null;
    }

    public void CreateOrder(string customerName)
    {
        this.customerName = customerName;
        order = GetRandomRecipe();
        orderActive = true;
        wantsToOrderAgain = false;
        OnOrderCreated?.Invoke(customerName, order.GetDrinkName());
        if (GameEventManager.Instance)
            GameEventManager.Instance.TriggerEvent(GameEventManager.GameEvent.CustomerOrdered);
    }

    public void CompleteOrder()
    {
        if (!orderActive) return;
        orderActive = false;
        drinksServed++;
        tab += order.GetPrice();
        OnOrderCompleted?.Invoke(customerName);
        //Make NPC order another drink
        if (drinksServed < drinkLimit)
        {
            wantsToOrderAgain = true;
        }
    }

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
            foreach (DrinkComponent spirit2 in drinkSpirits)
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
        List<Ingredient> recipeGarnishes = recipe.GetGarnishes();
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
    public bool WantsToOrderAgain() { return wantsToOrderAgain; }
    public bool HasFinishedAllDrinks() { return drinksServed >= drinkLimit; }
    public float GetTab() { return tab; }
}