using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Tier
{
    [SerializeField] List<Ingredient> ingredients;
    public List<Ingredient> GetIngredients()
    {
        return ingredients;
    }
}
public class RecipeController : MonoBehaviour
{
    [SerializeField] Tier[] tiers; // tiers (or levels) can maybe be scriptable objects
                                   // so we can persist and easily decouple
                                   // their data from this script. We would just need 
                                   // to pass the tier to the methods here
                                   // And this script itself can be static / singleton
                                   // or even a regular class so we don't need to always
                                   // have one in the scene

    [SerializeField] string PATH = "Bar"; // For testing purposes only

    private static RecipeController _instance;
    public static RecipeController Instance { get { return _instance; } }
    [SerializeField] bool disableSingleton = false;

    private void Awake()
    {
        if (disableSingleton)
        {
            return;
        }
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public bool VerifyRecipeControllerSetup()
    {
        Ingredient[] allIngredients = Resources.LoadAll<Ingredient>(PATH + "/Ingredients");

        HashSet<Ingredient> hashedIngredients = new HashSet<Ingredient>();
        bool foundAll = true;

        // Build hash set and detect duplicates in one pass
        foreach (Tier tier in tiers)
        {
            foreach (Ingredient ingredient in tier.GetIngredients())
            {
                // HashSet.Add returns false if item already exists
                if (!hashedIngredients.Add(ingredient))
                {
                    Debug.Log($"[RecipeController] Duplicate ingredient '{ingredient.GetName()}' found.");
                    foundAll = false;
                }
            }
        }

        // Check for unused ingredients
        foreach (Ingredient ingredient in allIngredients)
        {
            if (!hashedIngredients.Contains(ingredient))
            {
                Debug.Log($"[RecipeController] {ingredient.GetName()} is not currently assigned to recipe controller.");
                foundAll = false;
            }
        }

        return foundAll;
    }

    public List<Ingredient> GetIngredientsUnlockedAtLevel(int level)
    {
        if (tiers.Length >= level)
        {
            return tiers[level].GetIngredients();
        }
        return null;
    }
    public IEnumerable<Ingredient> GetAllUnlockedIngredients(int level)
    {
        //TODO: Can we run these operations more efficiently by caching results?
        if (tiers.Length > level)
        {
            foreach (Ingredient i in tiers[level].GetIngredients())
                yield return i;

            if (level > 0)
            {
                foreach (Ingredient ingredient in GetAllUnlockedIngredients(level - 1))
                    yield return ingredient;
            }
        }
    }
    public IEnumerable<Recipe> GetAllUnlockedRecipes(int level)
    {
        //TODO: Can we run these operations more efficiently by caching results?
        Recipe[] allRecipes = Resources.LoadAll(PATH + "/Recipes", typeof(Recipe)).Cast<Recipe>().ToArray();

        foreach (Recipe recipe in allRecipes)
        {
            if (recipe.CanMakeDrink(GetAllUnlockedIngredients(level)))
            {
                yield return recipe;
            }
        }
    }

    public IEnumerable<Recipe> GetNewlyUnlockedRecipes(int level)
    {
        IEnumerable<Recipe> oldRecipes = level > 0 ? GetAllUnlockedRecipes(level-1) : new List<Recipe>();
        IEnumerable<Recipe> newRecipes = GetAllUnlockedRecipes(level);
        return newRecipes.Except(oldRecipes);
    }

    public bool IngredientIsUnlocked(Ingredient ingredient, int level)
    {
        IEnumerable<Ingredient> unlockedIngredients = GetAllUnlockedIngredients(level);
        return unlockedIngredients.Contains(ingredient);
    }
}

#if UNITY_EDITOR
// Custom property drawer for SpawnSchedule list
[CustomPropertyDrawer(typeof(Tier))]
public class TierDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the index from the property path
        string path = property.propertyPath;
        int index = -1;

        // Extract index from path like "spawnSchedules.Array.data[0]"
        if (path.Contains("[") && path.Contains("]"))
        {
            int startIndex = path.IndexOf("[") + 1;
            int length = path.IndexOf("]") - startIndex;
            if (int.TryParse(path.Substring(startIndex, length), out index))
            {
                label.text = "Tier " + index;
            }
        }

        // Draw the property with custom label
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif