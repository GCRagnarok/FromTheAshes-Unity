using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replicadie : MonoBehaviour
{
    public GameObject boss;


    // Update is called once per frame
    void Update()
    {
        if(boss.GetComponent<BossHealth>().currentHealth <= 0)
        {
            GetComponent<Animator>().SetBool("IsDead", true);
            Invoke("Die", 0.5f);
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
