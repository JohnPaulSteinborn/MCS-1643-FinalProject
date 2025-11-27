using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed;
    private float moveInput;
    public bool isGrounded;
    private Rigidbody2D rb;
    public BoxCollider2D groundCollider;

    public PhysicsMaterial2D bounceMat, normalMat;
    public bool canJump = true;
    public float jumpValue = 0.0f;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        //if (jumpValue == 0.0f && isGrounded)
        if (jumpValue < 0.001f && IsGrounded())
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
        }

        //implement oncollisionenter
        if(jumpValue > 0)
        {
            rb.sharedMaterial = bounceMat;
        }
        else
        {
            rb.sharedMaterial = normalMat;
        }

        if (IsGrounded())
        {

            if (Input.GetKey("space") && canJump)
            {
                jumpValue += 0.2f;
            }

            if (Input.GetKeyDown("space") && canJump)
            {
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
            }

            if (jumpValue >= 20f)
            {
                float tempx = moveInput * walkSpeed;
                float tempy = jumpValue;
                rb.velocity = new Vector2(tempx, tempy);
                Invoke("ResetJump", 0.2f);
            }
            if (Input.GetKeyUp("space"))
            {
                rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
                jumpValue = 0.0f;
                canJump = true;
            }
        }
    }

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0;
    }

    private bool IsGrounded()
    {
        return groundCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
