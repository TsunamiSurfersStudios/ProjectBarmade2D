using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.2f;
    public int damage = 25;
    public float attackCooldown = 1.5f;

    [Header("Refs")]
    Rigidbody2D rb;
    EnemyRecoil recoil;
    private float lastAttackTime;

    [Header("External Forces")]
    public float externalDecay = 0.1f; // Decay rate for external forces
    private Vector2 externalForce; // Accumulated external force

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        recoil = GetComponent<EnemyRecoil>();

    }

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

        if (recoil != null && recoil.isRecoiling) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > attackRange)
        {
            // Move toward player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

            // Decay external force over time so we recover from knockback
            externalForce = Vector2.Lerp(externalForce, Vector2.zero, externalDecay * Time.deltaTime);

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

    public void ApplyExternalForce(Vector2 dir, float force)
    {
        externalForce += dir.normalized * force;
    }
}
