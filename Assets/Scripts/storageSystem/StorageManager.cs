using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public StorageSystem backStorage;
    public StorageSystem barStorage;

    // Add drink to the back storage
    public void AddDrinkToBack(Ingredient drink, float amount)
    {
        StorageSystem.Storage existing = backStorage.storageList
             .Find(s => s.drink == drink);

        if (existing != null)
        {
            existing.quantity += amount;
        }
        else
        {
            backStorage.storageList.Add(new StorageSystem.Storage(drink, amount));
        }
    }

    // Move drink from one storage to another
    public void MoveDrink(StorageSystem from, StorageSystem to, Ingredient drink, float amount)
    {
        // find the source entry
        StorageSystem.Storage source = from.storageList.Find(s => s.drink == drink);

        if (source == null || source.quantity < amount)
        {
            Debug.Log("Not enough drink in source storage");
            return;
        }

        // subtract from source
        source.quantity -= amount;

        // remove if empty
        if (source.quantity <= 0f)
            from.storageList.Remove(source);

        // find destination entry
        StorageSystem.Storage destination = to.storageList.Find(s => s.drink == drink);

        if (destination != null)
        {
            destination.quantity += amount;
        }
        else
        {
            to.storageList.Add(new StorageSystem.Storage(drink, amount));
        }
    }
}
