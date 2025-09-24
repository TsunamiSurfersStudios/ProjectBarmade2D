using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenGlass : HoldableObject
{
    [SerializeField] private TrashCan trashCan;
    [SerializeField] GameObject glass;
    private bool isHolding;
    
    public void Interact()
    {
        if(isHolding == false)
        {
            Debug.Log("Holding");
            Give();
            isHolding = true;
        }
        else if (isHolding)
        {
            Debug.Log("Dropping");
            itemHolder.DropItem();
            isHolding = false;
        }
    }
}