using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy9 : MonoBehaviour
{
    public bool doNotSpawn;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        doNotSpawn = (PlayerPrefs.GetInt("doNotSpawn9value") != 0);

        if (doNotSpawn == true)
        {
            this.enemy.GetComponent<EnemyStats>().died = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("doNotSpawn9value", (doNotSpawn ? 1 : 0));

        if (this.enemy.GetComponent<EnemyStats>().died == true)
        {
            doNotSpawn = true;
        }
    }
}
