using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NPCOrderingTests
{
    private NPCOrdering npcOrdering;
    private List<GameObject> testObjects;

    [SetUp]
    public void Setup()
    {
        testObjects = new List<GameObject>();
        GameObject testObject = new GameObject("TestNPCOrdering");
        testObjects.Add(testObject);
        npcOrdering = testObject.AddComponent<NPCOrdering>();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject obj in testObjects)
        {
            if (obj != null)
            {
                Object.DestroyImmediate(obj);
            }
        }
        testObjects.Clear();
    }

    private List<Ingredient> CreateTestIngredients()
    {
        var ingredients = new List<Ingredient>();
        ingredients.Add(Ingredient.Create("Rum", IngredientType.SPIRIT, 0.4f));
        ingredients.Add(Ingredient.Create("Coke", IngredientType.MIXER, 0));
        ingredients.Add(Ingredient.Create("Lime", IngredientType.GARNISH, 0));
        ingredients.Add(Ingredient.Create("Cherry", IngredientType.GARNISH, 0));
        return ingredients;
    }

    private Recipe CreateTestRecipe(bool hasIce = true)
    {
        var ingredients = CreateTestIngredients();
        var rum = ingredients[0];
        var coke = ingredients[1];
        var lime = ingredients[2];
        var cherry = ingredients[3];

        List<DrinkComponent> spirits = new List<DrinkComponent> { DrinkComponent.Create(rum, 10) };
        List<DrinkComponent> mixers = new List<DrinkComponent> { DrinkComponent.Create(coke, 10) };
        List<Ingredient> garnishes = new List<Ingredient> {lime, cherry };

        return Recipe.Create("Vodka and Coke", spirits, mixers, garnishes,
            Glass.ROCKS, hasIce, false);
    }

    private DrinkController CreateEmptyDrink()
    {
        GameObject drinkObject = new GameObject("TestDrink");
        testObjects.Add(drinkObject);
        return drinkObject.AddComponent<DrinkController>();
    }

    private DrinkController CreateDrinkFromRecipe(Recipe recipe, int multiplier = 1)
    {
        DrinkController drink = CreateEmptyDrink();

        // Add spirits
        foreach (DrinkComponent spirit in recipe.GetSpirits())
        {
            drink.AddIngredient(spirit.GetIngredient(), spirit.GetMilliliters() * multiplier);
        }

        // Add mixers
        foreach (DrinkComponent mixer in recipe.GetMixers())
        {
            drink.AddIngredient(mixer.GetIngredient(), mixer.GetMilliliters() * multiplier);
        }

        // Add garnishes
        foreach (Ingredient garnish in recipe.GetGarnishes())
        {
            drink.AddGarnish(garnish);
        }

        // Add ice if recipe requires it
        if (recipe.HasIce())
        {
            drink.AddIce();
        }

        drink.SetGlass(recipe.GetGlass());

        return drink;
    }

    private DrinkController CreatePartialDrink(Recipe recipe, bool skipSpirits = false,
        bool skipMixers = false, bool skipGarnishes = false, bool skipIce = false, bool wrongGlass = false)
    {
        DrinkController drink = CreateEmptyDrink();

        if (!skipSpirits)
        {
            foreach (DrinkComponent spirit in recipe.GetSpirits())
            {
                drink.AddIngredient(spirit.GetIngredient(), spirit.GetMilliliters());
            }
        }

        if (!skipMixers)
        {
            foreach (DrinkComponent mixer in recipe.GetMixers())
            {
                drink.AddIngredient(mixer.GetIngredient(), mixer.GetMilliliters());
            }
        }

        if (!skipGarnishes)
        {
            foreach (Ingredient garnish in recipe.GetGarnishes())
            {
                drink.AddGarnish(garnish);
            }
        }
        else
        {
            drink.AddGarnish(recipe.GetGarnishes()[0]);
        }

        if (!skipIce)
        {
            if (recipe.HasIce())
            {
                drink.AddIce();
            }
        }

        Glass recipeGlass = recipe.GetGlass();
        Glass glass = wrongGlass ? recipeGlass + 1 : recipeGlass;
        drink.SetGlass(glass);

        return drink;
    }

    [Test]
    public void GetRecipeAccuracy_EmptyDrink_ReturnsZero()
    {
        Recipe recipe = CreateTestRecipe();
        DrinkController emptyDrink = CreateEmptyDrink();

        float accuracy = npcOrdering.GetRecipeAccuracy(recipe, emptyDrink);

        Assert.AreEqual(0, accuracy);
    }

    [Test]
    public void GetRecipeAccuracy_MissingIngredient()
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreatePartialDrink(testRecipe, skipSpirits: true);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.65f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_WrongIngredient() 
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreateDrinkFromRecipe(testRecipe);
        Ingredient vodka = Ingredient.Create("Vodka", IngredientType.SPIRIT, 0.4f);
        drink.AddIngredient(vodka);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.85f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_WrongAmount() 
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreateDrinkFromRecipe(testRecipe, 2);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.7f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_MissingGarnish() 
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreatePartialDrink(testRecipe, skipGarnishes: true);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.95f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_IncorrectGarnish() 
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreateDrinkFromRecipe(testRecipe);
        Ingredient gumdrop = Ingredient.Create("Gumdrop", IngredientType.GARNISH, 0);
        drink.AddIngredient(gumdrop);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.95f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_MissingIce() 
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreatePartialDrink(testRecipe, skipIce: true);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.9f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_ExtraIce()
    {
        Recipe testRecipe = CreateTestRecipe(hasIce: false);
        DrinkController drink = CreatePartialDrink(testRecipe);
        drink.AddIce();

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.9f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_WrongGlass()
    {
        Recipe testRecipe = CreateTestRecipe(hasIce: false);
        DrinkController drink = CreatePartialDrink(testRecipe, wrongGlass: true);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(0.9f, accuracy, 0.01f);
    }

    [Test]
    public void GetRecipeAccuracy_PerfectDrink_ReturnsOne() 
    {
        Recipe testRecipe = CreateTestRecipe();
        DrinkController drink = CreateDrinkFromRecipe(testRecipe);

        float accuracy = npcOrdering.GetRecipeAccuracy(testRecipe, drink);

        Assert.AreEqual(1f, accuracy, 0.01f);
    }

    // TODO: Implement tests for glass types
    //TODO: Implement tests to check for duplicate ingredients
}
