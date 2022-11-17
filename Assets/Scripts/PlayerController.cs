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
    private int jumpCount;
    private int facing = 1;

    [Space(5)]
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 12f;

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

        #region Jump
        // Jump mechanic. Checks if you're either grounded or have an extra jump left. (Double Jump)
        if (Input.GetButtonDown("Jump") && (IsGrounded() || jumpCount > 0))
        {
            player.velocity = new Vector2(player.velocity.x, jumpPower);
            jumpCount--;
        }
        // Halves your vertical momentum when you let go of the button to make for more precise jumps.
        if (Input.GetButtonUp("Jump") && player.velocity.y > 0)
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.5f);
        }
        #endregion

        if (horizontal < 0) facing = -1;
        else if (horizontal > 0) facing = 1;

        if (Input.GetButtonDown("Fire1"))
        {
            bullet.GetComponent<Bullet>().dirX = facing;
            Instantiate(bullet, new Vector2(transform.position.x + (facing * 0.75f), transform.position.y), Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        #region Horizontal Movement
        // This confuses me so much. You're telling me if I try to make it so that you CAN walk when you're NOT against a wall
        // it completely breaks it and turns you all slippery, but when I make it so you CANT walk when you ARE against a wall
        // it works like normal? bruh.
        if (TouchingR() && horizontal > 0) { }
        else if (TouchingL() && horizontal < 0) { }
        else
        {
            player.velocity = new Vector2(horizontal * speed, player.velocity.y);
        }
        // it just works
        #endregion
    }

    #region Ground/Wall Checks
    // These all do the same, they draw a box the same size of the empty object in the editor and return true
    // when they collide with something.
    private bool IsGrounded()
    {
        // Had to add an extra if statement here so it doesn't constantly run jumpCount = 2.
        if (Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0, groundLayer))
        {
            jumpCount = 2;
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
}
