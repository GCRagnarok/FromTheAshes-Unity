using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIcicle : MonoBehaviour
{
    public GameObject icicle;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.GetComponent<EnemySpawnIcicle>().candamage == true)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        if(enemy.GetComponent<EnemySpawnIcicle>().candamage == false)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
