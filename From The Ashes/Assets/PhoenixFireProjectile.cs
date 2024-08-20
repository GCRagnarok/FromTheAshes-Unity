using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoenixFireProjectile : MonoBehaviour
{
    public int damage;

    public float speed = 10f;

    public Rigidbody2D rb;

    public GameObject enemy;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Enemies"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Fire"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Ice"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Boss"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Projectile"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Barrier"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("Mushroom"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("IcicleDamage"))
        {
            DestroyProjectile();
        }
        if (collision.gameObject.tag == ("IcicleWarning"))
        {
            DestroyProjectile();
        }

    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
