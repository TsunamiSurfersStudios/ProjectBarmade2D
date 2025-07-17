using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSpawner : MonoBehaviour
{
    public GameObject Drink;
    public Vector2 spawnPosition;
    // Start is called before the first frame update


    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject drinkClone = Instantiate(Drink, spawnPosition, Quaternion.identity);
            drinkClone.SetActive(true); // Show the drink
        }
    }
}
