using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RecipeControllerTests
{
    private const string PATH = "Assets/Scenes/Testing/RecipeController.unity";

    RecipeController controller;

    [SetUp]
    public void SetUp()
    {
        controller = GameObject.FindObjectOfType<RecipeController>();
    }

    [Test]
    [LoadScene(PATH)]
    public void VerifyPlayerMovementComponent()
    {
        controller = GameObject.FindObjectOfType<RecipeController>();
        Assert.IsNotNull(controller, "RecipeController component not found in the scene.");
    }

    [Test]
    public void VerifyAllIngredientsUsedTest()
    {
        bool allUsed = controller.VerifyAllIngredientsUsed();
        Assert.IsTrue(allUsed, "Not all ingredients are used in the RecipeController.");
    }
}
