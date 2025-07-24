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

    [Test]
    public void GetRandomRecipe_ReturnsRecipe()
    {
        Recipe recipe = npcOrdering.GetRandomRecipe();

        Assert.IsNotNull(recipe);
    }

    [Test]
    public void GetRandomRecipe_RecipeIsUnlocked()
    {
        Recipe randomRecipe = npcOrdering.GetRandomRecipe();
        bool isUnlocked = randomRecipe.getUnlocked();
        Assert.True(isUnlocked);
    }

    [Test]
    public void GetRecipeAccuracy_EmptyDrink_ReturnsZero()
    {
        Recipe randomRecipe = npcOrdering.GetRandomRecipe();

        GameObject drinkObject = new GameObject("TestDrink");
        testObjects.Add(drinkObject);
        DrinkController emptyDrink = drinkObject.AddComponent<DrinkController>();

        float accuracy = npcOrdering.GetRecipeAccuracy(randomRecipe, emptyDrink);

        Assert.AreEqual(0, accuracy);
    }
}