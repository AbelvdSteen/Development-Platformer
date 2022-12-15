using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 15f;
    private float lifeTime = 3f;

    [SerializeField] private Transform collisionCheck;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask groundLayer;

    public int dirX;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        transform.Translate(transform.right * dirX * speed * Time.deltaTime);

        Collider2D enemy = Physics2D.OverlapBox(transform.position, transform.localScale, 0, enemyLayer);

        // Change to: enemy.GetComponent<BossScript>().Damage();
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().Die();
            Destroy(gameObject);
        }

        // Makes sure the arrow gets destroyed after hitting an object if said object isn't an enemy.
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, groundLayer)) Destroy(gameObject);
    }
}
