using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public GameObject player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.GetComponent<PlayerController>().isDead == false)
        {
            print("poison");
            player.GetComponent<PlayerController>().isDead = true;
            player.GetComponent<PlayerController>().Die();
        }
    }
}
