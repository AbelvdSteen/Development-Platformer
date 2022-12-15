using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField] private int facing;
    [SerializeField] private float speed;

    [Space(10)]
    [SerializeField] Rigidbody2D enemy;
    [SerializeField] private Transform wallCheckL;
    [SerializeField] private Transform wallCheckR;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameManager gameManager;

    [Space(10)]
    [SerializeField] private GameObject dieParticle;
    #endregion

    void FixedUpdate()
    {
        if (TouchingL()) facing = 1;
        if (TouchingR()) facing = -1;
        enemy.velocity = new Vector2(speed * facing, enemy.velocity.y);

        Collider2D player = Physics2D.OverlapBox(transform.position, transform.localScale, 0, playerLayer);
        if (player != null)
        {
            player.GetComponent<PlayerController>().Die();
        }
    }

    private bool TouchingL()
    {
        return Physics2D.OverlapBox(wallCheckL.position, wallCheckL.localScale, 0, groundLayer);
    }
    private bool TouchingR()
    {
        return Physics2D.OverlapBox(wallCheckR.position, wallCheckR.localScale, 0, groundLayer);
    }

    public void Die()
    {
        // Add a sound and a particle or something

        gameManager.AddScore(150);
        Instantiate(dieParticle, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

        Debug.Log("You got an enemy!");
        Destroy(gameObject);
    }
}
