using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dishwasher : MonoBehaviour
{
    private ItemHolder itemHolder;

    private void Start()
    {
        itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
    }

    public IEnumerator WashDishes()
    { 
        if (!itemHolder.IsEmpty()) { 
            itemHolder.DestroyObject();
            Debug.Log("Washing dishes...");
            yield return new WaitForSeconds(5);
            Debug.Log("Dishes are clean");
        }
    }
}

/* TODO: 
 * Remove functionality from Update function
 * Use InteractionController instead of controlling interactions from dishwasher script
 * Turn ItemHolder into a class so we can call itemholder.isEmpty();
 * dirtyGlass should have a getter/setter instead of being publicly exposed
 * We should create a HoldableObject class that 'Glass' can inherit from that has a function to 'hand object to player'
 * Revisit "coroutine" and examine peak otimized way to acheive this
 */