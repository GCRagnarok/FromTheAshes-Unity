using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnProjectile : MonoBehaviour
{
    private float timeBtwShots;
    public float startTimeBtwShots;
    public bool canInstantiate;
    public bool spawnedReplica;

    public GameObject projectile;
    public Transform player;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;


    public bool setActive;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<BossHealth>().currentHealth <= 100)
        {
            if(setActive == false)
            {
                spawnPoint1.SetActive(true);
                spawnPoint2.SetActive(true);
                setActive = true;
                GetComponent<BossSpawnIcicle>().canInstantiate = false;
            }
            if (setActive == true)
            {
                return;
            }
        }
    }
}
