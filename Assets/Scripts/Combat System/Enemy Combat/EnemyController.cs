using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("External Forces")]
    private Rigidbody2D rb;
    private Vector2 anchorPosition;
    public float externalDecay = 0.1f; // Decay rate for external forces
    private Vector2 externalForce; // Accumulated external force
    float nextHitTime;
    public float speed = 2f; // Speed of the enemy when chasing the player

    [Header("Recoil Settings")]
    public float recoilDistance = 0f; // How far the enemy is pushed back
    public float recoilDuration = 2.5f; // How long the recoil lasts    
    public float hitCooldown = 2f; // Minimum time between hits
    public bool isRecoiling = false;

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

        anchorPosition = rb.position;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.isKinematic = true;

        SetAggro(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        // Return to anchor position if not aggro
        if (!isAggro)
        {
            //Vector2 pos = rb.position;
            //Vector2 to = anchorPosition - pos;
            //float dist = to.magnitude;
            //Vector2 dir = dist > 0.001f ? to / dist : Vector2.zero;
            //if (dist > 0.1f)
            //{
            //    rb.MovePosition(pos + dir * follow.speed * Time.fixedDeltaTime);
            //}

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.MovePosition(anchorPosition);
        }

        //enemy follow
        if (!player || !isAggro) return;

        Vector2 pos = rb.position;
        Vector2 to = (Vector2)player.position - pos;
        float dist = to.magnitude;
        Vector2 dir = dist > 0.001f ? to / dist : Vector2.zero;

        if (dist > stopDistance && !isRecoiling)
        {
            rb.MovePosition(pos + dir * speed * Time.fixedDeltaTime);
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
        //rb.isKinematic = !aggro;

        if (!aggro)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.MovePosition(anchorPosition);
        }
        else
        {
            // When becoming aggro, do nothing special for now
            //Debug.Log("Enemy " + gameObject.name + " is now aggro.");
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

        if (isAggro && Time.time >= nextHitTime)
        {
            nextHitTime = Time.time + hitCooldown;
            ApplyRecoil(location);
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
    public void ApplyRecoil(Vector2 direction)
    {
        //if (Time.time < nextHitTime) return;
        if (!isRecoiling)
        {
            StartCoroutine(RecoilCoroutine(direction));
            isRecoiling = false;
            Debug.Log("Enemy " + gameObject.name + " recoiled.");
        }
        nextHitTime = Time.time + hitCooldown;
    }

    System.Collections.IEnumerator RecoilCoroutine(Vector2 direction)
    {
        isRecoiling = true;

        Vector2 dir = direction.normalized; // move away from the hitter
        Vector2 start = rb.position;
        Vector2 end = start + dir * recoilDistance;

        float t = 0f;
        while (t < recoilDuration)
        {
            Debug.Log("Enemy is recoiling...");
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / recoilDuration);
            // Ease-out (feels punchier): u = 1 - (1-u)^2
            u = 1f - (1f - u) * (1f - u);

            rb.MovePosition(Vector2.Lerp(start, end, u));
            yield return null;
        }

        isRecoiling = false; // chase can resume
    }
}
