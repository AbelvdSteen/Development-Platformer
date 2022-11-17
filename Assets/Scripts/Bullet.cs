using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 15f;
    private float lifeTime = 3f;

    [SerializeField] private Transform collisionCheck;

    public int dirX;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        transform.Translate(transform.right * dirX * speed * Time.deltaTime);

        if (Physics2D.OverlapBox(collisionCheck.position, collisionCheck.localScale, 0)) // this will not differentiate between enemies and wall lmao sorry future me
        {
            Destroy(gameObject);
        }
    }
}
