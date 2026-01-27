using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDrink()
    {
        GameObject drink = GameObject.Find("Drink");
        Instantiate(drink);
        
    }
    

}
