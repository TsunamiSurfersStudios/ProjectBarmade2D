using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //enemy health
    public float currentHealth = 100f;
    private float maxHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth <= 0)
        {
            Die(); // Call Die method if health is zero or below
        }
    }

    private void Die()
    {
        // Handle enemy death (e.g., play animation, drop loot, etc.)
        Debug.Log("Enemy has died.");
        Destroy(gameObject); // Destroy the enemy game object
    }
}
