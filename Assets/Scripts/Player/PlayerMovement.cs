using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool movementEnabled;

    Rigidbody2D rb; 
    private Vector2 movement;
    private Animator mAnimator; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovemnt();
        HandleAnimations();
    }

    void HandleMovemnt()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); 
        movement.y = Input.GetAxisRaw("Vertical");
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
        if(movementEnabled) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}

