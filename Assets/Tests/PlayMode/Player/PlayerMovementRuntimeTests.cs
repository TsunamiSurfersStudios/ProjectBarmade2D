using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerMovementRuntimeTests
{
    GameObject player;
    PlayerMovement playerMovement;

    [SetUp]
    public void SetUp()
    {
        player = GameObject.Find("PlayerBartender");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.StartTesting();
    }

    [Test]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public void VerifyApplicationPlaying()
    {
        Assert.That(Application.isPlaying, Is.True);
    }

    [Test]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public void VerifyPlayerMovementComponent()
    {
        PlayerMovement playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        Assert.IsNotNull(playerMovement, "PlayerMovement component not found in the scene.");
    }

    [UnityTest]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public IEnumerator HandleMovement_MoveUp()
    {
        Vector3 initialPosition = player.transform.position;
     
        playerMovement.SetMovement(0f, 1f);
        yield return new WaitForFixedUpdate();

        Assert.Greater(player.transform.position.y, initialPosition.y);
    }

    [UnityTest]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public IEnumerator HandleMovement_MoveDown()
    {
        Vector3 initialPosition = player.transform.position;

        playerMovement.SetMovement(0f, -1f);
        yield return new WaitForFixedUpdate();

        Assert.Less(player.transform.position.y, initialPosition.y);
    }

    [UnityTest]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public IEnumerator HandleMovement_MoveLeft()
    {
        Vector3 initialPosition = player.transform.position;

        playerMovement.SetMovement(-1f, 0f);
        yield return new WaitForFixedUpdate();

        Assert.Less(player.transform.position.x, initialPosition.x);
    }


    [UnityTest]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public IEnumerator HandleMovement_MoveRight()
    {
        Vector3 initialPosition = player.transform.position;

        playerMovement.SetMovement(1f, 0f);
        yield return new WaitForFixedUpdate();

        Assert.Greater(player.transform.position.x, initialPosition.x);
    }
}

public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction
{
    string scene;

    public LoadSceneAttribute(string scene)
    {
        this.scene = scene;
    }

    public IEnumerator BeforeTest(ITest test)
    {
        Debug.Assert(scene.EndsWith(".unity"), "Scene name must end with .unity");
        yield return EditorSceneManager.LoadSceneInPlayMode(scene, new LoadSceneParameters(LoadSceneMode.Single));
    }

    public IEnumerator AfterTest(ITest test)
    {
        yield return null;
    }
}