using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShadowProjectile : MonoBehaviour
{
    private float timeBtwShots;
    public float startTimeBtwShots;
    public bool canInstantiate;

    public GameObject projectile;
    public Transform player;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        canInstantiate = true;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwShots <= 0 && canInstantiate == true)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
            audioSource.Play();
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
