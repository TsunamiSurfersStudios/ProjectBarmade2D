using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDrink", menuName = "Inventory/Drink")]
public class Drink : ScriptableObject
{
    public string drinkName;
    public float alcoholPercent;
    public float maxVolume;



}
