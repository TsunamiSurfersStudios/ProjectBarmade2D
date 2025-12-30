using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 1.0f;
    public float attackRange = 1.2f;
    public int damage = 25;
    public float attackCooldown = 3f;

    [Header("Enemy Health")]
    public float currentHealth = 100f;
    public float maxHealth = 100f;

    [Header("Attack Stuff")]
    private float lastAttackTime;
    private bool isAggro = false;
    public bool isDead = false;
    private bool isRecoiling = false;

    [Header("External Forces")]
    private Rigidbody2D rb;
    //private Vector2 anchorPosition;
    float nextHitTime;
    public float hitCooldown = 2f; // Minimum time between hits
    public float speed = 2f; // Speed of the enemy when chasing the player

    [Header("Recoil Settings")]
    public float recoilDistance = 10f; // How far the enemy is pushed back
    public float recoilDuration = 1f; // How long the recoil lasts    

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Initialize current health to max health

        // Find the player by tag
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        //anchorPosition = rb.position;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0f;
        SetAggro(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (isRecoiling)
        {
            return; // Skip movement while recoiling
        }

        //enemy follow
        if (!player || !isAggro) return;

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
            rb.velocity = Vector2.zero;
        }
    }

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

    //Aggro attack
    void AttackPlayer()
    {
        // Assuming the player has a PlayerHealth script to handle damage
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void SetAggro(bool aggro)
    {
        if (isDead) return;

        isAggro = aggro;

        if (!aggro)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            //rb.MovePosition(anchorPosition);
        }
        else
        {
            // When becoming aggro, do nothing special for now
            Debug.Log("Enemy " + gameObject.name + " is now aggro.");
        }
    }

    //enemy health
    public void TakeDamage(float damage, Vector2 location)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        EnemyController enemyFollow = GetComponent<EnemyController>();
        if (enemyFollow != null)
        {
            enemyFollow.SetAggro(true); // Set isAggro to true when taking damage
        }

        if (isAggro)
        {
            Vector2 direction = ((Vector2)transform.position - location);
            ApplyRecoil(direction);
        }

        if (currentHealth <= 0)
        {
            if (enemyFollow != null)
            {
                enemyFollow.Die();
            }
            else
            {
                Die();
            }
        }
     }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        SetAggro(false);
        // Additional death handling can be added here
        Debug.Log("Enemy " + gameObject.name + " has died.");
        Destroy(gameObject, 2f); // Destroy after 2 seconds to allow for death animation
    }

    //enemy recoil
    public void ApplyRecoil(Vector2 sender)
    {
        StopAllCoroutines();
        if (!isRecoiling)
            StartCoroutine(RecoilCoroutine(sender));
    }

    private IEnumerator RecoilCoroutine(Vector2 go)
    {
        isRecoiling = true;
        Vector2 direction = go.normalized * recoilDistance;
        direction = rb.velocity;
        yield return new WaitForSeconds(recoilDuration);
        rb.velocity = Vector2.zero;
        isRecoiling = false;
    }
}
