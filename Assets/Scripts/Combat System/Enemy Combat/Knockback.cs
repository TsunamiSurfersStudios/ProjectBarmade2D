using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float Distance = 10f; // How far the enemy is pushed back
    public float Duration = 1f; // How long the recoil lasts    

    private Rigidbody2D rb;

    public UnityEvent OnBegin, OnDone;

    public void KnockFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke(); // signal that recoil is starting
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb.AddForce(direction * Distance, ForceMode2D.Impulse); // apply recoil force in the opposite of the player
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(Duration);
        rb.velocity = Vector3.zero;
        OnDone?.Invoke(); // signal that recoil is done
    }
}
