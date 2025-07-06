using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int damage = 25;
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mouseWorldPos - transform.position).normalized;

        // Detect enemies in a small arc/cone or point
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Hit enemy with melee!");
            }
        }

        Debug.DrawRay(transform.position, attackDirection * attackRange, Color.red, 0.5f); // Debug line
    }
}
