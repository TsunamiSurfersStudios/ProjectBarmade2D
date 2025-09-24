using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class TrashBag : HoldableObject
{   
    void Update()
    {
        
    }
    
    public void GiveBag()
    {
        Debug.Log("Holding gameObject");
        Give();
    }

    public void DropBag()
    {
        Debug.Log("Dropping gameObject");
        itemHolder.DropItem();
    }

    public void SpawnBag()
    {
        Spawn();
    }

    public void DestroyBag()
    {
        itemHolder.DestroyObject();
    }
}