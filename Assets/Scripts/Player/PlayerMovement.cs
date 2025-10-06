using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 movement;
    private bool inTestMode;

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
        Animator mAnimator = GetComponent<Animator>();
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

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        HandleAnimations();
    }
    public void StartTesting()
    {
        inTestMode = true;
    }
}

