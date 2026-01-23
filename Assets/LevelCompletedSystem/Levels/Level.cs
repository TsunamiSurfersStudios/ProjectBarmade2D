using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] List<Recipe> recipeRewards;
    [SerializeField] List<Ingredient> ingredientRewards;
}
