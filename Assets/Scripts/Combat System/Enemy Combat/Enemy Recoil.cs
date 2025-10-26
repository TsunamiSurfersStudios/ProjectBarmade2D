using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecoil : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float recoilDistance = 5f; // How far the enemy is pushed back
    public float recoilDuration = 2.5f; // How long the recoil lasts    
    public float hitCooldown = 2f; // Minimum time between hits
    public bool isRecoiling = false;

    float nextHitTime;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyRecoil(Vector2 direction)
    {
        if (Time.time < nextHitTime) return;
        if (!isRecoiling) 
        {
            StartCoroutine(RecoilCoroutine(direction));
            //isRecoiling = false;
        }
        nextHitTime = Time.time + hitCooldown;
    }

    System.Collections.IEnumerator RecoilCoroutine(Vector2 direction)
    {
        isRecoiling = true;

        Debug.Log("Enemy recoiling!");
        Vector2 dir = direction.normalized; // move away from the hitter
        Vector2 start = rb.position;
        Vector2 end = start + dir * recoilDistance;

        float t = 0f;
        while (t < recoilDuration)
        {
            Debug.Log("Recoil in progress...");
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / recoilDuration);
            // Ease-out (feels punchier): u = 1 - (1-u)^2
            u = 1f - (1f - u) * (1f - u);

            rb.MovePosition(Vector2.Lerp(start, end, u));
            yield return null;
        }

        isRecoiling = false; // chase can resume
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
