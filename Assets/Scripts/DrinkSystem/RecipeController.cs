using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tier
{
    [SerializeField] Ingredient[] ingredients;
    public Ingredient[] GetIngredients()
    {
        return ingredients;
    }
}
public class RecipeController : MonoBehaviour
{
    [SerializeField] Tier[] tiers;
    /// Design a list where you can drag in ingredients
    /// Class should be able to pring all recipes that can be unlocked at a specific tier (put this in tests)
    /// class should print any unused recipes/ingredients (put this in tsts)
    /// class should track current leve and return avaliable ingredients
    /// given an ingredient, return boolean if it is unlocked
    /// unlock new level ==> returns int
    /// given level num, return all recipes that are unlocked at that level
    /// given level num, return all ingredients that are unlocked at that level
    /// given level num, return all unlocked ingredients/recipes
}
