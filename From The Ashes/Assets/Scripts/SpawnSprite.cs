using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSprite : MonoBehaviour
{
    public GameObject deathSprite;
    private PlayerController player;

    public bool spawned;


    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player.isDead == true && player.isGrounded == true && spawned == false && player.travellerForm == false)
        {
            spawned = true;
            Invoke("SpawnAtPlayerLocation", 0.1f);
        }
    }

    void SpawnAtPlayerLocation()
    {
        GameObject instance = (GameObject)Instantiate(deathSprite, transform.position, Quaternion.identity);
    }
}
