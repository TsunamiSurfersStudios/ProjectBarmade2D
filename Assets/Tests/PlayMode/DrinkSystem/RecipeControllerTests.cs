using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RecipeControllerTests
{
    private const string SCENE_PATH = "Assets/Scenes/Testing/RecipeController.unity";
    private const string RECIPE_PATH = "TestBar/Recipes/";
    private const string INGREDIENT_PATH = "TestBar/Ingredients/";

    RecipeController controller;

    Recipe ALLINGREDIENTS;
    Recipe EMPTYRECIPE;
    Recipe ONESPIRIT;
    Recipe ONESPIRITONEMIXER;
    Recipe ONESPIRITONEMIXERONEGARNISH;
    Recipe ONESPIRITTWOMIXERS;

    List<Recipe> LevelZeroRecipes;
    List<Recipe> LevelOneRecipes;
    List<Recipe> LevelTwoRecipes;
    List<Recipe> LevelThreeRecipes;

    [SetUp]
    public void SetUp()
    {
        GameObject recipeObject = GameObject.Find("RecipeController");
        controller = recipeObject.GetComponent<RecipeController>();

        ALLINGREDIENTS = Resources.Load<Recipe>(RECIPE_PATH + "AllIngredients");
        EMPTYRECIPE = Resources.Load<Recipe>(RECIPE_PATH + "EmptyRecipe");
        ONESPIRIT = Resources.Load<Recipe>(RECIPE_PATH + "OneSpirit");
        ONESPIRITONEMIXER = Resources.Load<Recipe>(RECIPE_PATH + "OneSpiritOneMixer");
        ONESPIRITONEMIXERONEGARNISH = Resources.Load<Recipe>(RECIPE_PATH + "OneSpiritOneMixerOneGarnish");
        ONESPIRITTWOMIXERS = Resources.Load<Recipe>(RECIPE_PATH + "OneSpiritTwoMixers");

        LevelZeroRecipes = new List<Recipe> { EMPTYRECIPE, ONESPIRIT };
        LevelOneRecipes = new List<Recipe> { ONESPIRITONEMIXER };
        LevelTwoRecipes = new List<Recipe> { ONESPIRITONEMIXERONEGARNISH, ONESPIRITTWOMIXERS };
        LevelThreeRecipes = new List<Recipe> { ALLINGREDIENTS };
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    public void VerifyPlayerMovementComponent()
    {
        controller = GameObject.FindObjectOfType<RecipeController>();
        Assert.IsNotNull(controller, "RecipeController component not found in the scene.");
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    public void VerifyAllIngredientsUsedTest_HasAllIngredients_ReturnsTrue()
    {
        bool allUsed = controller.VerifyAllIngredientsUsed();
        Assert.IsTrue(allUsed, "Not all ingredients are used in the RecipeController.");
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    public void VerifyAllIngredientsUsedTest_IsEmpty_ReturnsFalse()
    {
        GameObject emptyObject = GameObject.Find("EmptyRecipeController");
        RecipeController emptyController = emptyObject.GetComponent<RecipeController>();
        bool allUsed = emptyController.VerifyAllIngredientsUsed();
        Assert.IsFalse(allUsed, "Empty controller is returning that all ingredients are used.");
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    public void VerifyAllIngredientsUsedTest_HasDuplicate_ReturnsFalse()
    {
        GameObject duplicateObject = GameObject.Find("DuplicateRecipeController");
        RecipeController duplicateController = duplicateObject.GetComponent<RecipeController>();
        bool allUsed = duplicateController.VerifyAllIngredientsUsed();
        Assert.IsFalse(allUsed, "Duplicate controller is returning that all ingredients are used.");
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    [TestCase(0, new string[] { "Spirit1" })]
    [TestCase(1, new string[] { "Spirit1", "Spirit2", "Mixer1" })]
    [TestCase(2, new string[] { "Spirit1", "Spirit2", "Mixer1", "Spirit3", "Mixer2", "Garnish1" })]
    [TestCase(3, new string[] { "Spirit1", "Spirit2", "Mixer1", "Spirit3", "Mixer2", "Garnish1", "Garnish2" })]
    public void GetAllUnlockedIngredients_HasAllIngredients(int level, string[] unlockedIngredientNames)
    {
        List<Ingredient> unlockedIngredients = new List<Ingredient>();
        foreach (string name in unlockedIngredientNames)
        {
            Ingredient ingredient = Resources.Load<Ingredient>(INGREDIENT_PATH + name);
            Assert.IsNotNull(ingredient, $"Ingredient '{name}' could not be loaded.");
            unlockedIngredients.Add(ingredient);
        }

        IEnumerable<Ingredient> ingredients = controller.GetAllUnlockedIngredients(level);
        List<Ingredient> ingredientList = new List<Ingredient>(ingredients);
        CollectionAssert.AreEquivalent(unlockedIngredients, ingredientList, $"Level {level} unlocked ingredients do not match expected.");
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    [TestCase(0, new string[] { "EmptyRecipe", "OneSpirit" })]
    [TestCase(1, new string[] { "EmptyRecipe", "OneSpirit", "OneSpiritOneMixer" })]
    [TestCase(2, new string[] { "EmptyRecipe", "OneSpirit", "OneSpiritOneMixer", "OneSpiritOneMixerOneGarnish", "OneSpiritTwoMixers" })]
    [TestCase(3, new string[] { "EmptyRecipe", "OneSpirit", "OneSpiritOneMixer", "OneSpiritOneMixerOneGarnish", "OneSpiritTwoMixers", "AllIngredients" })]
    public void GetAllUnlockedRecipes_HasAllRecipes(int level, string[] unlockedRecipeNames)
    {
        List<Recipe> unlockedRecipes = new List<Recipe>();

        foreach (string name in unlockedRecipeNames)
        {
            Recipe recipe = Resources.Load<Recipe>(RECIPE_PATH + name);
            Assert.IsNotNull(recipe, $"Recipe '{name}' could not be loaded.");
            unlockedRecipes.Add(recipe);
        }

        IEnumerable<Recipe> recipes = controller.GetAllUnlockedRecipes(level);
        List<Recipe> recipeList = new List<Recipe>(recipes);
        CollectionAssert.AreEquivalent(unlockedRecipes, recipeList, $"Level {level} unlocked recipes do not match expected.");
    }

    [Test]
    [LoadScene(SCENE_PATH)]
    [TestCase(0, new string[] { "EmptyRecipe", "OneSpirit" })]
    [TestCase(1, new string[] { "OneSpiritOneMixer" })]
    [TestCase(2, new string[] { "OneSpiritOneMixerOneGarnish", "OneSpiritTwoMixers" })]
    [TestCase(3, new string[] { "AllIngredients" })]
    [TestCase(4, new string[] {})]
    public void GetNewlyUnlockedRecipes_HasCorrectRecipes(int level, string[] expectedRecipeNames)
    {
        List<Recipe> expectedRecipeList = new List<Recipe>();

        foreach (string name in expectedRecipeNames)
        {
            Recipe recipe = Resources.Load<Recipe>(RECIPE_PATH + name);
            Assert.IsNotNull(recipe, $"Recipe '{name}' could not be loaded.");
            expectedRecipeList.Add(recipe);
        }

        IEnumerable<Recipe> recipes = controller.GetNewlyUnlockedRecipes(level);
        List<Recipe> recipeList = new List<Recipe>(recipes);
        CollectionAssert.AreEquivalent(expectedRecipeList, recipeList, $"Level {level} newly unlocked recipes do not match expected.");
    }

}
