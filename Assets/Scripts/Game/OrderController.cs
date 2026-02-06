using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    private GameObject orderFrame;
    // Start is called before the first frame update
    void Start()
    {
        if(orderFrame == null)
        {
            orderFrame = GameObject.FindInParent("Order");
        } else
        {
            Debug.LogError("Order Frame not found in children of " + gameObject.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
