using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float acceleration = 1f;
    [SerializeField] float decceleration = 1f;
    [SerializeField] float maxHorizontalVelocity = 1f;
    [SerializeField] float maxVerticalVelocity = 1f;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] int maxJumps = 2;
    [Tooltip("Time after jumping before downwards force is applied")]
    [SerializeField] float airTimeBeforeApplyingExtraGravity = 1f;
    [Tooltip("Gravity that is applied to make the player spend less time going down than going up after jumping")]
    [SerializeField] float extraGravity = 1f;
    [Tooltip("Distance from to player to ground before player is considered grounded")]
    [SerializeField] float isGroundedOffset = 0.1f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioClipSO jumpSound;
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] ParticleSystem landParticles;

    bool isJumpPressed;
    bool isGrounded;
    bool isGroundedThisFrame;
    bool shouldApplyExtraGravity = true;
    float movementDirectionInput;
    float airTime;
    int jumpCount;

    private void Update()
    {
        CheckForInput();

        if (transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movementDirectionInput = 1f;
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                animator.SetBool("isPlayerMoving", true);
                spriteRenderer.flipX = true;
            }
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirectionInput = -1f;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                animator.SetBool("isPlayerMoving", true);
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            animator.SetBool("isPlayerMoving", false);
        }
    }

    void MovePlayer()
    {
        bool isMovementInput = movementDirectionInput > 0.1 || movementDirectionInput < -0.1;
        bool horizontalVelocityIsZero = rb.velocity.x > 0.1 || rb.velocity.x < -0.1;
        bool overMaxHorizontalVelocity = Mathf.Abs(rb.velocity.x) >= maxHorizontalVelocity;
        bool overMaxVerticalVelocity = Mathf.Abs(rb.velocity.y) >= maxVerticalVelocity;

        LayerMask groundMask = LayerMask.GetMask("Wall");
        RaycastHit2D isGroundedHit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, Vector2.down, isGroundedOffset, groundMask);

        // Friction + freeze rotation is bugged; this is a hack fix
        rb.angularVelocity = 0f;

        if (isGroundedHit)
        {
            if (!isGrounded)
            {
                isGroundedThisFrame = true;
            }
            else
            {
                isGroundedThisFrame = false;
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isMovementInput)
        {
            bool inputAndVelocityAreInSameDirection = (movementDirectionInput > 0 && rb.velocity.x > 0) || (movementDirectionInput < 0 && rb.velocity.x < 0);
            if (inputAndVelocityAreInSameDirection && ! overMaxHorizontalVelocity)
            {
                Accelerate();
            }
            else
            {
                Deccelerate();
            }
        }

        if (horizontalVelocityIsZero && !isMovementInput)
        {
            Deccelerate();
        }

        if (overMaxHorizontalVelocity)
        {
            ClampHorizontalVelocity();
        }

        if (isGrounded)
        {
            jumpCount = maxJumps;
            if (isGroundedThisFrame)
            {
                landParticles.Play();
            }
        }

        if (isJumpPressed && jumpCount > 0)
        {
            Jump();
        }

        if (!isGrounded && shouldApplyExtraGravity && !overMaxVerticalVelocity)
        {
            ApplyExtraGravity();
        }

        if (overMaxVerticalVelocity)
        {
            ClampVerticalVelocity();
        }

        ResetInput();
    }

    void ResetInput()
    {
        isJumpPressed = false;
        movementDirectionInput = 0f;
    }

    void Jump()
    {
        StopCoroutine("co_JumpTimer");
        jumpCount--;
        isGrounded = false;
        isGroundedThisFrame = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        AudioManager.instance.PlayAudioClip(jumpSound);
        jumpParticles.Play();
        StartCoroutine("co_JumpTimer");
    }

    IEnumerator co_JumpTimer()
    {
        shouldApplyExtraGravity = false;
        airTime = 0f;

        while (!isGrounded && airTime < airTimeBeforeApplyingExtraGravity)
        {
            airTime += Time.deltaTime;
            yield return null;
        }

        shouldApplyExtraGravity = true;
    }

    void ApplyExtraGravity()
    {
        rb.AddForce(Vector2.down * extraGravity);
    }

    void ClampHorizontalVelocity()
    {
        float horizontalDirection = GetHorizontalDirection();
        float maxVelocityX = maxHorizontalVelocity * horizontalDirection;
        rb.velocity.Set(maxVelocityX, rb.velocity.y);
    }

    void ClampVerticalVelocity()
    {
        float horizontalDirection = Mathf.Sign(rb.velocity.y);
        float maxVelocityY = maxVerticalVelocity * horizontalDirection;
        rb.velocity.Set(rb.velocity.x, maxVelocityY);
    }

    void Accelerate()
    {
        rb.AddForce(Vector2.right * movementDirectionInput * acceleration);
    }

    void Deccelerate()
    {
        float oppositeHorizontalDirection = -GetHorizontalDirection();

        rb.AddForce(Vector2.right * oppositeHorizontalDirection * decceleration);
    }

    float GetHorizontalDirection ()
    {
        float oppositeHorizontalDirection = 0f;
        if (rb.velocity.x > 0)
        {
            oppositeHorizontalDirection = Mathf.Ceil(rb.velocity.x);
        }
        else if (rb.velocity.x < 0)
        {
            oppositeHorizontalDirection = Mathf.Floor(rb.velocity.x);
        }

        return oppositeHorizontalDirection;
    }
}


