using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ItemHolderTests
{
    GameObject gameObject; 
    ItemHolder itemHolder;
    GameObject testItem;

    [SetUp]
    public void SetUp()
    {
        gameObject = new GameObject("itemHolder");
        itemHolder = gameObject.AddComponent<ItemHolder>();
        testItem = new GameObject("TestItem");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameObject);
        Object.DestroyImmediate(testItem);
    }

    [Test]
    public void GiveItem_HolderNotEmpty()
    {
        itemHolder.GiveObject(testItem);

        Assert.IsFalse(itemHolder.IsEmpty());
    }

    [Test]
    public void DropItem_HolderEmpty()
    {
        itemHolder.DropItem();

        Assert.IsTrue(itemHolder.IsEmpty());
    }


    [Test]
    public void GiveObject_TakeSameObjectBack()
    {
        itemHolder.GiveObject(testItem);
        GameObject takenItem = itemHolder.TakeObject();

        Assert.AreEqual(testItem, takenItem);
    }

    [Test]
    public void GiveObject_TakeObject_HolderEmtpy()
    {
        itemHolder.GiveObject(testItem);
        itemHolder.TakeObject();

        Assert.IsTrue(itemHolder.IsEmpty());
    }

    [Test]
    public void GiveObject_DropItem_HolderEmtpy()
    {
        itemHolder.GiveObject(testItem);
        itemHolder.DropItem();

        Assert.IsTrue(itemHolder.IsEmpty());
    }
}
