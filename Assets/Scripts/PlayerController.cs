using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Apparently, unless you want other scripts to access them, you generally shouldn't make variables public in case something
    // messes up on another script. Hence, the serialization. It's pretty much just an extra safety measure.

    // I learned this in a Youtube tutorial so correct me if I'm wrong LMAO
    #region Movement Variables
    private float horizontal;
    [SerializeField] private int jumpCount;
    private int facing = 1;

    [Space(5)]
    [Header("Movement")]

    [SerializeField] private bool doubleJump = true;

    [Space(10)]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private float jumpBuffer;
    private float jumpBufferCounter;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [Space(10)]
    [SerializeField] private bool wallSliding;
    [SerializeField] private float wallSlideSpeed;

    [Space(10)]
    [SerializeField] private bool wallJumping;
    [SerializeField] private float xWallForce;
    [SerializeField] private float yWallForce;
    [SerializeField] private float wallJumpTime;

    [Space(10)]
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private LayerMask groundLayer;

    [Space(10)]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckL;
    [SerializeField] private Transform wallCheckR;
    #endregion

    #region Combat Variables
    [Space(10)]
    [Header("Combat")]
    [SerializeField] private GameObject bullet;
    #endregion

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        #region Jumping
        // Coyote Time
        if (IsGrounded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // Jump Buffer
        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter -= Time.deltaTime;
        
        // Jump mechanic. Checks if you're either grounded or have an extra jump left. (Double Jump)
        if (jumpBufferCounter > 0 && (coyoteTimeCounter > 0f || jumpCount > 0) && !(TouchingL() || TouchingR()))
        {
            player.velocity = new Vector2(player.velocity.x, jumpPower);
            if (coyoteTimeCounter <= 0) jumpCount--; // Only remove the jumpCount if it's actually using a double jump.
            jumpBufferCounter = 0f; // Prevents spamming

            // I don't know why, but you sometimes randomly get back your jumps when using a walljump and a normal jump doesn't subtract from it.
            // Hence why jumpCount is 1 instead of 2. Idk.
        } 

        // Wall Jump mechanic.
        else if (Input.GetButtonDown("Jump") && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
            // See Line 111 for the actual wallJump mechanic
        }

        // Halves your vertical momentum when you let go of the button to make for more precise jumps.
        if (Input.GetButtonUp("Jump") && player.velocity.y > 0)
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.5f);
            coyoteTimeCounter = 0f; // Prevents spamming
        }

        #endregion

        #region Shooting

        if (Input.GetButtonDown("Fire1"))
        {
            bullet.GetComponent<Bullet>().dirX = facing;
            Instantiate(bullet, new Vector2(transform.position.x + (facing * 0.75f), transform.position.y), Quaternion.identity);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region Horizontal Movement
        // This confuses me so much. You're telling me if I try to make it so that you CAN walk when you're NOT against a wall
        // it completely breaks it and turns you all slippery, but when I make it so you CANT walk when you ARE against a wall
        // it works like normal? bruh.

        // Also checks if you're not currently in a wallJump animation, otherwise its all janky.
        if (!(TouchingR() && horizontal > 0) && !(TouchingL() && horizontal < 0) && !wallJumping)
        {
            player.velocity = new Vector2(horizontal * speed, player.velocity.y);

            if (horizontal < 0) facing = -1;
            else if (horizontal > 0) facing = 1;
        }
        // it just works


        // Checks if you are not grounded and touching a wall on any side, then halves your vertical speed and allows you to walljump.
        if (!IsGrounded() && (TouchingL() || TouchingR()))
        {
            player.velocity = new Vector2(player.velocity.x, Mathf.Clamp(player.velocity.y, -wallSlideSpeed, float.MaxValue));
            wallSliding = true;
        } else
        {
            wallSliding = false;
        }
        #endregion

        // I wanted to put this in FixedUpdate in case of deltaTime shenanigans, refers back to Line 60-64.
        if (wallJumping) player.velocity = new Vector2(xWallForce * -facing, yWallForce);
    }

    #region Ground/Wall Checks
    // These all do the same, they draw a box the same size of the empty object in the editor and return true
    // when they collide with something.
    private bool IsGrounded()
    {
        // Had to add an extra if statement here so it doesn't constantly run jumpCount = 2.
        if (Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0, groundLayer))
        {
            if (doubleJump) jumpCount = 1;
            return true;
        }
        return false;
    }
    private bool TouchingL()
    {
        return Physics2D.OverlapBox(wallCheckL.position, wallCheckL.localScale, 0, groundLayer);
    }
    private bool TouchingR()
    {
        return Physics2D.OverlapBox(wallCheckR.position, wallCheckR.localScale, 0, groundLayer);
    }
    #endregion

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
}
