using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsRequired7 : MonoBehaviour
{
    public GameObject player;
    public Animator anim;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().enemiesDefeated >= 7)
        {
            anim.SetTrigger("Burn");
            Invoke("SetActiveFalse", 0.6f);
        }
    }
    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void BurnSound()
    {
        audioSource.Play();
    }
}

