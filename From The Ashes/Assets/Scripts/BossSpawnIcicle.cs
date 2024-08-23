using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnIcicle : MonoBehaviour
{
    private float timeBtwShots;
    public float startTimeBtwShots;
    public bool canInstantiate;
    public float offset;

    public GameObject icicleWarning;
    public GameObject icicleDamage;
    public Transform player;
    public Transform ice1;
    public Transform ice2;
    public Transform ice3;
    public Transform ice4;
    public Transform ice5;
    public Transform ice6;
    public Transform ice7;
    public Transform ice8;
    public Transform ice9;
    public Transform ice10;
    public Transform ice11;
    public Transform ice12;
    AudioSource audioSource;

    public bool candamage;
    public bool spawned;


    // Start is called before the first frame update
    void Start()
    {
        spawned = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;

        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        if (timeBtwShots <= 0 && canInstantiate == true  && GameObject.FindGameObjectWithTag("IcicleDamage") == null && spawned == false)
        {
            Instantiate(icicleWarning, ice1.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice2.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice3.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice4.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice5.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice6.transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
            Invoke("EnableHitbox1", 0.5f);
            Invoke("SetSpawned", 2.0f);
        }
        if (timeBtwShots <= 0 && canInstantiate == true  && GameObject.FindGameObjectWithTag("IcicleDamage") == null && spawned == true)
        {
            Instantiate(icicleWarning, ice7.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice8.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice9.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice10.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice11.transform.position, Quaternion.identity);
            Instantiate(icicleWarning, ice12.transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
            Invoke("EnableHitbox2", 0.5f);
            Invoke("SetSpawned", 2.0f);
        }
        if (timeBtwShots <= 0 && GetComponent<BossHealth>().currentHealth <= 100)
        {
            SetSpawned();
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
            
        }
    }
    void EnableHitbox1()
    {
        Destroy(GameObject.FindGameObjectWithTag("IcicleWarning"));
        Instantiate(icicleDamage, ice1.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice2.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice3.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice4.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice5.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice6.transform.position, Quaternion.identity);
        audioSource.Play();

    }
    void EnableHitbox2()
    {
        Destroy(GameObject.FindGameObjectWithTag("IcicleWarning"));
        Instantiate(icicleDamage, ice7.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice8.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice9.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice10.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice11.transform.position, Quaternion.identity);
        Instantiate(icicleDamage, ice12.transform.position, Quaternion.identity);
        audioSource.Play();

    }

    void SetSpawned()
    {
        if(spawned == true)
        {
            spawned = false;
        }
        else
        {
            spawned = true;
        }
    }
}
