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
    private float percentage = 0f; // max: 1
    private bool containsIce = false;

    void Start()
    {
        itemHolder = GameObject.FindWithTag("Player").GetComponentInChildren<ItemHolder>();
        if (percentage > 1)
        {
            Debug.Log(name + " alcohol percentage exceeds 100%. Scripts may not work as intended.");
        }
    }

    public void GiveDrink()
    {
        Debug.Log("give drink");
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
        return percentage;
    }

    public void AddIngredient(Ingredient ingredient, int milliliters)
    {
        Ingredient newIngredient = Instantiate(ingredient);
        IngredientType type = newIngredient.GetIngredientType();
        if (type == IngredientType.SPIRIT || type == IngredientType.MIXER)
        {
            DrinkComponent drink = new DrinkComponent();
            drink.AddIngredient(newIngredient);
            drink.AddMilliliters(milliliters);
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
            percentage = 0;
            foreach (float volume in volumes)
            {
                percentage += volume / totalVolume;
            }
        }
        else
        {
            garnishes.Add(newIngredient);
        }
    }
}
