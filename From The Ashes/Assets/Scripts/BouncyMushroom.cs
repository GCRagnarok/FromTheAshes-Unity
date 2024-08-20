using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyMushroom : MonoBehaviour
{
    public Animator anim;
    public PlayerController player;

    void Update()
    {
        if(player.GetComponent<PlayerController>().mushroomBounce == true)
        {

            anim.SetTrigger("Bounce");
        }
    }
}
