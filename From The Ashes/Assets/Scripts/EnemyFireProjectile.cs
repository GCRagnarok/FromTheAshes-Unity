using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool canFire;

    public Transform enemy;
    private Transform player;
    private Vector2 target;
    private Vector2 target2;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);

        target2 = new Vector2(enemy.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(canFire == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
       
        if(transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
            DestroyProjectile();
        }
        if (other.CompareTag("Ground"))
        {
            DestroyProjectile();
        }
        if (other.CompareTag("PhoenixProjectile"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
