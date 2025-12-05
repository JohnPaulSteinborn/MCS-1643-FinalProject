using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 5f;
    private float moveInput;

    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float jumpValue = 0f;
    public bool canJump = true;

    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Move only when not charging a jump
        if (jumpValue == 0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundMask);

        if (!isGrounded || jumpValue > 0)
        {
            rb.sharedMaterial = bounceMat;
        }
        else
        {
            rb.sharedMaterial = normalMat;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && canJump)
        {
            jumpValue += 0.2f;
            // Reset horizontal velocity for consistency
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // When jumpValue reaches full charge and you're still grounded
        if (jumpValue >= 20f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
            Invoke(nameof(ResetJump), 0.2f);
        }

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

    private void ResetJump()
    {
        canJump = false;
        jumpValue = 0;
    }
}
