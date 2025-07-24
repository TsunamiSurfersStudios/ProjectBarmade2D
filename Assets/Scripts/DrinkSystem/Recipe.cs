using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDrinkRecipe", menuName = "Bar/DrinkRecipe")]
public class Recipe : ScriptableObject
{
    [Header("Drink Info")]
    [SerializeField] string drinkName;
    [SerializeField] DrinkComponent[] spirits;
    [SerializeField] DrinkComponent[] mixers;
    [SerializeField] Ingredient[] garnishes;
    [SerializeField] Glass glass;
    [SerializeField] bool hasIce;
    [SerializeField] bool isBlended;
    [Header("Sale Info")]
    [SerializeField] float price;
    [SerializeField] bool isUnlocked;

    public bool getUnlocked()
    { return isUnlocked; }
}
