using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleDamage : MonoBehaviour
{
    public GameObject player;
    public int attackDamage;

    private void Start()
    {
        attackDamage = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            print("hit");
            player.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }
}

