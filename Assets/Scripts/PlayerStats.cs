using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool holding;

   
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public bool getHolding(){
        return holding;
    }

    public void changeHolding(){
        holding = !holding;
    }
}
