using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    // Start is called before the first frame update
    public void GetIce()
    {
        ItemHolder holder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        Debug.Log("Refilling from ice machine");
        GameObject ice = new GameObject("Ice"); // TODO: we should not be using name to check if it is ice or not
        holder.GiveObject(ice);
    }
}
