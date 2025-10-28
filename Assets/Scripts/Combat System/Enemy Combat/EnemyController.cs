using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Enemy State
    public bool isAggro = false;
    public bool isDead = false;

    //components
    private EnemyFollow follow;
    private EnemyHealth health;
    private EnemyRecoil recoil;
    private Rigidbody2D rb;

    //anchor for enemies
    private Vector2 anchorPosition;

    // Start is called before the first frame update
    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        follow = GetComponent<EnemyFollow>();
        recoil = GetComponent<EnemyRecoil>();
        rb = GetComponent<Rigidbody2D>();

        //checks
        if (!health || !follow || !recoil)
        {
            Debug.LogError("EnemyHealth component not found on " + gameObject.name);
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
    }

    public void SetAggro(bool aggro)
    {
        if (isDead) return;

        isAggro = aggro;
        follow.enabled = aggro;
        rb.isKinematic = !aggro;
        recoil.enabled = aggro;

        if (!aggro)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.MovePosition(anchorPosition);
        }
        else
        {
            // When becoming aggro, do nothing special for now
            Debug.Log("Enemy " + gameObject.name + " is now aggro.");
        }
    }

    //enemyhealth
    public void Die()
    {
        isDead = true;
        follow.enabled = false;
        recoil.enabled = false;
        rb.velocity = Vector2.zero;
        SetAggro(false);
        // Additional death handling can be added here
        Debug.Log("Enemy " + gameObject.name + " has died.");
        Destroy(gameObject, 2f); // Destroy after 2 seconds to allow for death animation
    }

}
