using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;

    public float walkSpeed = 5f;
    private float moveInput;

    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float jumpValue = 0f;
    public float maxJumpValue = 15f;
    public bool canJump = true;

    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;

    private Rigidbody2D rb;

    private AudioSource audioSrc;
    public AudioClip jumpSound;
    public AudioClip groundSound;
    public AudioClip chargeSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Move only when not charging a jump
        if (jumpValue == 0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            } 
            else if (moveInput < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        bool left = Physics2D.Raycast(transform.position + new Vector3(-0.45f, 0, 0), Vector2.down, 0.6f, groundMask);
        bool mid = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundMask);
        bool right = Physics2D.Raycast(transform.position + new Vector3(0.45f, 0, 0), Vector2.down, 0.6f, groundMask);

        isGrounded = left || mid || right;

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
            jumpValue += 0.25f;
            jumpValue = Mathf.Clamp(jumpValue, 0f, maxJumpValue);
            // Reset horizontal velocity for consistency
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            audioSrc.PlayOneShot(chargeSound);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            audioSrc.PlayOneShot(jumpSound);
        }

        // When jumpValue reaches full charge and you're still grounded
        if (jumpValue >= maxJumpValue && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
            Invoke(nameof(ResetJump), 0.0f);
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

        anim.SetFloat("moveSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("verticalVelocity", rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isGrounded)
        {
            audioSrc.PlayOneShot(groundSound);
        }
    }

    private void ResetJump()
    {
        canJump = false;
        jumpValue = 0;
    }
}
