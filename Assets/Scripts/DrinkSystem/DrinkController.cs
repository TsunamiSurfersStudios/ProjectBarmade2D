using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class DrinkController : MonoBehaviour
{
    public GameObject drink;
    private ItemHolder itemHolder;

    // Drink creation
    private List<DrinkComponent> spirits = new List<DrinkComponent>();
    private List<DrinkComponent> mixers = new List<DrinkComponent>();
    private List<Ingredient> garnishes = new List<Ingredient>();
    private Glass glass;
    private float alcoholPercentage = 0f; // max: 1
    private bool hasIce = false;

    void Start()
    {
        itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        if (alcoholPercentage > 1)
        {
            Debug.Log(name + " alcohol percentage exceeds 100%. Scripts may not work as intended.");
        }
    }

    public void GiveDrink()
    {
        itemHolder.GiveObject(gameObject);
    }
    public void SpawnDrink()
    {
        GameObject clone = GameObject.Instantiate(drink);
        itemHolder.GiveObject(clone);
        clone.SetActive(true);
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
            DrinkComponent drink = DrinkComponent.Create(newIngredient, milliliters);
            if (type == IngredientType.SPIRIT) { spirits.Add(drink); }
            else { mixers.Add(drink); }

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

    public void AddIce()
    {
        hasIce = true;
    }

    public List<DrinkComponent> GetSpirits() { return spirits; }
    public List<DrinkComponent> GetMixers() { return mixers; }
    public List<Ingredient> GetGarnishes() { return garnishes; }
    public Glass GetGlass() { return glass; }
    public bool HasIce() { return hasIce; }
    public void SetGlass(Glass glass) { this.glass = glass; }
}
