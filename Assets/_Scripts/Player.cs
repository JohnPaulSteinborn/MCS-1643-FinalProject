using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 5f;
    private float moveInput;

    public bool isGrounded;
    public float jumpValue = 0f;
    public bool canJump = true;

    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundCheckRadius = 0.2f;

    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        // Swap physics material
        col.sharedMaterial = (jumpValue > 0f) ? bounceMat : normalMat;

        HandleMovement();
        HandleJumpCharging();
        HandleJumpRelease();
    }

    void HandleMovement()
    {
        if (isGrounded && jumpValue == 0f)
        {
            // Normal walking on ground
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
        }
        else if (!isGrounded)
        {
            // Allows sliding off walls instead of sticking
            rb.velocity = new Vector2(moveInput * 1f, rb.velocity.y);
        }
    }

    void HandleJumpCharging()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && canJump)
        {
            jumpValue += 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            // Reset horizontal movement while charging
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // Automatically launch if charged too long
        if (jumpValue >= 20f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
            Invoke(nameof(ResetJump), 0.2f);
        }
    }

    void HandleJumpRelease()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
                jumpValue = 0f;
            }

            canJump = true;
        }
    }

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0;
    }
}
