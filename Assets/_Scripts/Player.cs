using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //movement
    public float moveSpeed = 4.0f;
    public float horizontalLaunchMultiplier = 4f;

    //jump charge
    public float maxJumpCharge = 1.5f;
    public float minJumpForce = 6f;
    public float maxJumpForce = 18f;

    //ground check
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool onGround = false;
    private float lastMoveInput = 0f;
    public float jumpCharge = 0f;
    private bool isCharging = false;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        
    }

    void GroundCheck()
    {
        //check if player is on ground
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void MovementControl()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (onGround)
        {
            rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
            if (input != 0)
            {
                lastMoveInput = input;
            }
        }
    }

    void JumpCharge()
    {
        if (onGround && Input.GetButtonDown("Jump"))
        {
            isCharging = true;
            jumpCharge = 0f;
        }
        if (isCharging && Input.GetButton("Jump"))
        {
            jumpCharge += Time.deltaTime;
            //puts max limit on player's jump charge
            jumpCharge = Mathf.Clamp(jumpCharge, 0f, maxJumpCharge);
        }
    }

    void JumpRelease()
    {
        if (isCharging && Input.GetButtonUp("Jump"))
        {
            isCharging = false;

            float t = jumpCharge / maxJumpCharge;
            //smooths position over jump
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, t);

            //vertical launch
            float verticalVel = jumpForce;

            //horizontal launch
            float horizontalVel = lastMoveInput * horizontalLaunchMultiplier;

            rb.velocity = new Vector2(horizontalVel, verticalVel);

            jumpCharge = 0f;
        }
    }
}
