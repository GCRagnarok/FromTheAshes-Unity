using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IceCanSearchTrigger : MonoBehaviour
{
    public GameObject enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.GetComponent<AIPath>().canSearch = true;
            enemy.GetComponent<EnemySpawnIcicle>().canInstantiate = true;
        }
    }
}
