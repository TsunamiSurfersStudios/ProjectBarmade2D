using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class BackRoomStorage : Ingredient
{
    [SerializeField] private GameObject backRoomObject;
}
public class StorageSystem : MonoBehaviour
{
    [System.Serializable]

    // holds Ingredients and the amount of ingredients
    public class BarStorage
    {
        public Ingredient drink;
        public float quantity;

        public BarStorage(Ingredient drink, float quantity)
        {
            this.drink = drink;
            this.quantity = quantity;
        }
    }

    public List<BarStorage> storage = new List<BarStorage>();

    public void AddDrink(Ingredient drink, int amount)
    {
        // checks to see if the item already exist in List storage
        var existingItem = storage.Find(i => i.drink == drink);

        // if the item does exist then add to the amount
        if (existingItem != null)
        {
            existingItem.quantity += amount;
        }
        else //adds a new item in the storage by creating a new DrinkSlot object
        {
            storage.Add(new BarStorage(drink, amount));
        }

    }

    public void RemoveDrink(Ingredient drink, int amount)
    {
        var existingItem = storage.Find(i => i.drink == drink);

        if (existingItem != null)
        {
            existingItem.quantity -= amount;
        }
    }

    // need to make a function that adds to back room then that adds to the bar storage
}

