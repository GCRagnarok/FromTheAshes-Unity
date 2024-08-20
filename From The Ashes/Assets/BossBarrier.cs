using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public Animator anim;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {    
        if (player.GetComponent<PlayerController>().phoenixForm == true)
        {
            anim.SetTrigger("Burn");
            Invoke("SetActiveFalse", 0.6f);
        }
    }
    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
