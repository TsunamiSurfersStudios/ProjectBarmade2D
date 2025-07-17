using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.2f;
    public int damage = 25;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;


    // Start is called before the first frame update
    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > attackRange)
        {
            // Move toward player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Try to attack the player
            if (Time.time > lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                AttackPlayer();
            }
        }

    }

    void AttackPlayer()
    {
        // Assuming the player has a PlayerHealth script to handle damage
        //PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        //if (playerHealth != null)
        //{
        //    playerHealth.TakeDamage(damage);
        //    Debug.Log("Enemy attacked the player for " + damage + " damage.");
        //}
    }
}
