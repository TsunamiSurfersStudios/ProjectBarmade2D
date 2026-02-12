using UnityEngine;

namespace Player
{
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
                mAnimator.SetBool("isBack", Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
                mAnimator.SetBool("isRight", Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
                mAnimator.SetBool("isForward", Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
                mAnimator.SetBool("isLeft", Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
            }

            // Adjust item holder positition
            bool isBackwards = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            ItemHolder.Instance.SwitchPosition(!isBackwards);
        }

        void FixedUpdate()
        {
            SetMovement();

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
}

