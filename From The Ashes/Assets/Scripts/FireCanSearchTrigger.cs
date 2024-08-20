using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FireCanSearchTrigger : MonoBehaviour
{
    public GameObject enemy;
    public GameObject projectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.GetComponent<AIPath>().canSearch = true;
            enemy.GetComponent<EnemySpawnProjectiles>().canInstantiate = true;
            projectile.GetComponent<EnemyFireProjectile>().canFire = true;
        }
    }
}