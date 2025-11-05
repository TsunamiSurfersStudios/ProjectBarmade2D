using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NPCControllerTests
{
    GameObject npc;
    NPCController controller;

    /*
    [SetUp]
    public void SetUp()
    {
        npc = GameObject.Find("NPC");
        npcController = npc.GetComponent<NPCController>();
    }
    */

    [Test]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public void VerifyApplicationPlaying()
    {
        Assert.That(Application.isPlaying, Is.True);
    }

    [Test]
    [LoadScene("Assets/Scenes/PlayTests.unity")]
    public void VerifyNPCControllerComponent()
    {
        NPCController controller = GameObject.FindObjectOfType<NPCController>();
        Assert.IsNotNull(controller, "NPC Controller component not found in the scene.");
    }
}
