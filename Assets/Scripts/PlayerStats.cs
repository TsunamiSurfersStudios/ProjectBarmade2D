using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool isPickingUp;

   
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public bool getPickingUp(){
        return isPickingUp;
    }

    public void changePickUp(){
        isPickingUp = !isPickingUp;
    }
}
