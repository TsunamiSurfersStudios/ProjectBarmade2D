using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerItem : DrinkController
{
    //public Sprite displayImage; Might bring this back later :/
    // Start is called before the first frame update

    public void SayName()
    {
        Debug.Log(gameObject.name);
    }
}
