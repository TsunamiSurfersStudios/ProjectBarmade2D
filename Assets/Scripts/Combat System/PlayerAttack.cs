using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage = 2.0f; // Range of the attack

    // Update is called once per frame
    void Update()
    {
        // 0 = Left Click Mouse Button
        // 1 = Right Click Mouse Button
        // 2 = Middle Click Mouse Button
        if (Input.GetMouseButtonDown(0)) // Check if the attack button is pressed
        {
            // Cast a ray forward to simulate a hit
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 2f);
            if (hit.collider != null)
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
