using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerItem : DrinkController
{
    public void SayName()
    {
        Debug.Log(gameObject.name);
    }
}
