using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class DrinkController : HoldableObject
{
    // Drink creation
    //TODO: Update final glass sprite based on selected glass and drink contents
    private List<DrinkComponent> spirits = new List<DrinkComponent>();
    private List<DrinkComponent> mixers = new List<DrinkComponent>();
    private List<Ingredient> garnishes = new List<Ingredient>();
    private Glass glass;
    private float alcoholPercentage = 0f; // max: 1
    private bool hasIce = false;//TODO: Rework this, ice shouldnt be a boolean it should be a float that keeps track of how much ice is in the drink. Some guests might ask for extra ice, some might ask for light ice
    new void Start()
    {
        base.Start(); //Assign itemHolder
        if (alcoholPercentage > 1)
        {
            Debug.Log(name + " alcohol percentage exceeds 100%. Scripts may not work as intended.");
        }
    }

    public void GiveDrink()
    {
        GiveToPlayer();
    }

    public void SpawnDrink(Ingredient spirit = null)
    {
        if (spirit != null)
        {
            AddIngredient(spirit, 355);//Add beer ingredient with 355 ml (12 oz)
        }
        Recipe recipe = Recipe.Create("Custom drink", spirits, mixers, garnishes, glass, hasIce, false);
        Spawn(clone =>
        {
            DrinkController cloneDrinkController = clone.GetComponent<DrinkController>();
            if (cloneDrinkController != null && recipe != null)
            {
                cloneDrinkController.InitializeFromRecipe(recipe);
            }
        });
        
        ResetDrink();
    }

    private void InitializeFromRecipe(Recipe recipe)
    {
        spirits.Clear();
        mixers.Clear();
        garnishes.Clear();

        // Add spirits from recipe
        foreach (DrinkComponent spirit in recipe.GetSpirits())
        {
            AddIngredient(spirit.GetIngredient(), spirit.GetMilliliters());
        }

        // Add mixers from recipe
        foreach (DrinkComponent mixer in recipe.GetMixers())
        {
            AddIngredient(mixer.GetIngredient(), mixer.GetMilliliters());
        }

        // Add garnishes from recipe
        foreach (Ingredient garnish in recipe.GetGarnishes())
        {
            AddGarnish(garnish);
        }

        SetGlass(recipe.GetGlass());
        if (recipe.HasIce())
        {
            AddIce();
        }
    }

    public float GetAlcoholPercentage()
    {
        return alcoholPercentage;
    }

    public void AddIngredient(Ingredient ingredient, int milliliters = 0)
    {
        Ingredient newIngredient = Instantiate(ingredient);
        IngredientType type = newIngredient.GetIngredientType();
        if (type == IngredientType.SPIRIT || type == IngredientType.MIXER)
        {
            DrinkComponent item = DrinkComponent.Create(newIngredient, milliliters);
            if (type == IngredientType.SPIRIT) { spirits.Add(item); }
            else { mixers.Add(item); }

            // Calculate percentage
            int totalVolume = 0;
            List<float> volumes = new List<float>();
            foreach (DrinkComponent spirit in spirits)
            {
                totalVolume += spirit.GetMilliliters();
                volumes.Add(spirit.GetAlcoholAmount());
            }
            foreach (DrinkComponent mixer in mixers)
            {
                totalVolume += mixer.GetMilliliters();
                volumes.Add(mixer.GetAlcoholAmount());
            }
            alcoholPercentage = 0;
            foreach (float volume in volumes)
            {
                alcoholPercentage += volume / totalVolume;
            }
        }
        else
        {
            garnishes.Add(newIngredient);
        }
    }

    public void SelectGlass(Glass glass)
    {
        this.glass = glass; 
    }
    public void AddGarnish(Ingredient ingredient)
    {
        AddIngredient(ingredient, 0);
    }

    public void AddIce()//TODO: Rework to use float for keeping track of how much ice is in the drink
    {
        hasIce = true;
    }

    public void ResetDrink()
    {
        spirits.Clear();
        mixers.Clear();
        garnishes.Clear();
        alcoholPercentage = 0f;
        hasIce = false;
    }

    public List<DrinkComponent> GetSpirits() { return spirits; }
    public List<DrinkComponent> GetMixers() { return mixers; }
    public List<Ingredient> GetGarnishes() { return garnishes; }
    public Glass GetGlass() { return glass; }
    public bool HasIce() { return hasIce; }
    public void SetGlass(Glass glass) { this.glass = glass; }
}
