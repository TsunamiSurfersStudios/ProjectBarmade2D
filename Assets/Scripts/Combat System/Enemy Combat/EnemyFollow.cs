using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 1.0f;
    public float attackRange = 1.2f;
    public int damage = 25;
    public float attackCooldown = 3f;

    [Header("Attack Stuff")]
    Rigidbody2D rb;
    EnemyRecoil recoil;
    private float lastAttackTime;
    public bool isAggro = false;

    [Header("External Forces")]
    public float externalDecay = 0.1f; // Decay rate for external forces
    private Vector2 externalForce; // Accumulated external force

    public float speed = 2f; // Speed of the enemy when chasing the player

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        recoil = GetComponent<EnemyRecoil>();


    }

    // Start is called before the first frame update
    void Start()
    {
        // Find the player by tag
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (!player || !isAggro) return;

        if (recoil != null && recoil.isRecoiling) return; // pause during jump back

        Vector2 pos = rb.position;
        Vector2 to = (Vector2)player.position - pos;
        float dist = to.magnitude;
        Vector2 dir = dist > 0.001f ? to / dist : Vector2.zero;

        if (dist > stopDistance)
        {
            rb.MovePosition(pos + dir * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero; // don’t “crawl” into the player collider
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player || !isAggro) return;

        // Attack check outside movement so it still runs when standing in range
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }

    }

    void AttackPlayer()
    {
        // Assuming the player has a PlayerHealth script to handle damage
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null )
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void ApplyExternalForce(Vector2 dir, float force)
    {
        externalForce += dir.normalized * force;
    }
}
