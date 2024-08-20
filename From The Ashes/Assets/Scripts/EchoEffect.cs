using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timeBtwSpawns;
    public float startTimeBtwEchoSpawns;
    public float startTimeBtwSmokeSpawns;

    public GameObject echo;
    public GameObject echo2;
    public GameObject smoke;
    public GameObject smoke2;
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player.dashing == true && player.facingRight)
        {
            if (timeBtwSpawns <= 0)
            {
                //spawn echo game object
                GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(instance, 0.3f);
                timeBtwSpawns = startTimeBtwEchoSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
        else if (player.dashing == true && !player.facingRight)
        {
            if (timeBtwSpawns <= 0)
            {
                //spawn echo game object
                GameObject instance = (GameObject)Instantiate(echo2, transform.position, Quaternion.identity);
                Destroy(instance, 1f);
                timeBtwSpawns = startTimeBtwEchoSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
        else if (player.isWallSliding == true && player.isTouchingLeftWall == true && player.travellerForm == true)
        {
            if (timeBtwSpawns <= 0)
            {
                //spawn echo game object
                GameObject instance = (GameObject)Instantiate(smoke, transform.position, Quaternion.identity);
                Destroy(instance, 0.3f);
                timeBtwSpawns = startTimeBtwSmokeSpawns;


            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
        else if (player.isWallSliding == true && player.isTouchingRightWall == true && player.travellerForm == true)
        {
            if (timeBtwSpawns <= 0)
            {
                //spawn echo game object
                GameObject instance = (GameObject)Instantiate(smoke2, transform.position, Quaternion.identity);
                Destroy(instance, 0.3f);
                timeBtwSpawns = startTimeBtwSmokeSpawns;

            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
