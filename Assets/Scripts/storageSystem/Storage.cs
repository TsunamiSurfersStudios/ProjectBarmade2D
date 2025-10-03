using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    [System.Serializable]
    public class DrinkSlot
    {
        public Drink drink;
        public float quantity;
    }

    public List<DrinkSlot> inventory = new List<DrinkSlot>();

    public void AddDrink(Drink drink, float amount)
    {
        foreach (var slot in inventory)
        {
            if (slot.drink == drink)
            {
                slot.quantity += amount;
                return;
            }
        }
        inventory.Add(new DrinkSlot { drink = drink, quantity = amount });
    }

    public bool TransferDrink(StorageSystem target, Drink drink, float amount)
    {
        foreach (var slot in inventory)
        {
            if (slot.drink == drink && slot.quantity >= amount)
            {
                slot.quantity -= amount;
                target.AddDrink(drink, amount);
                return true;
            }
        }
        return false;
    }
}

