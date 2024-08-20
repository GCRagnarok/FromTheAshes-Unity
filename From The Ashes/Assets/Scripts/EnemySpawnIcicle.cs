using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnIcicle : MonoBehaviour
{

    private float timeBtwShots;
    public float startTimeBtwShots;
    public bool canInstantiate;
    public float offset;

    public GameObject icicleWarning;
    public GameObject icicleDamage;
    public Transform player;
    public Transform iciclePos;
    AudioSource audioSource;

    public bool candamage;
    public bool spawn;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;

        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwShots <= 0 && canInstantiate == true && player.GetComponent<PlayerController>().isGrounded == true && GameObject.FindGameObjectWithTag("IcicleWarning") == null)
        {
            Instantiate(icicleWarning, player.transform.position, Quaternion.identity);
            Invoke("EnableHitbox", 0.5f);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        iciclePos = GameObject.FindGameObjectWithTag("IcicleWarning").transform;
    }
    void EnableHitbox()
    {
        Instantiate(icicleDamage, iciclePos.transform.position, Quaternion.identity);
        Destroy(GameObject.FindGameObjectWithTag("IcicleWarning"));
        audioSource.Play();
    }
}
