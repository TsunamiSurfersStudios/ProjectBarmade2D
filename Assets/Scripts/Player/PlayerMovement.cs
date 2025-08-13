using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    Rigidbody2D rb;
    private Vector2 movement;
    private Animator mAnimator;
    private bool inTestMode;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
    }

    void SetMovement()
    {
        if (!inTestMode)
        {
            SetMovement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
    public void SetMovement(float x, float y)
    {
        movement.x = x;
        movement.y = y;
        movement = movement.normalized;// Normalize movement to prevent faster diagonal movement
    }

    void HandleAnimations()
    {
        if (mAnimator) // TODO: Handle animations should be in a function
        {
            mAnimator.SetBool("isBack", Input.GetKey(KeyCode.W));
            mAnimator.SetBool("isRight", Input.GetKey(KeyCode.D));
            mAnimator.SetBool("isForward", Input.GetKey(KeyCode.S));
            mAnimator.SetBool("isLeft", Input.GetKey(KeyCode.A));
        }
    }

    void FixedUpdate()
    {
        SetMovement();
        HandleAnimations();

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    public void StartTesting()
    {
        inTestMode = true;
    }
}

