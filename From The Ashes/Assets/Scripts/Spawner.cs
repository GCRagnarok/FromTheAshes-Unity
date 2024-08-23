using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    private PlayerController playercontroller;

    // Start is called before the first frame update
    void Start()
    {
        playercontroller = GetComponent<PlayerController>();

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        GameObject a = Instantiate(player) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
