using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject player;
    public int heal = 25;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            print("collision");
            FindObjectOfType<AudioManager>().Play("Health");
            player.GetComponent<PlayerController>().currentHealth += heal;
            Destroy(this.gameObject);
        }
    }
}
