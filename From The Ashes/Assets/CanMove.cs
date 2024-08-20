using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMove : MonoBehaviour
{
    public GameObject boss;
    public bool begin;
    public GameObject barrier1;
    public GameObject barrier2;
    public GameObject barrier3;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("StartBattle", 0.2f);
            barrier1.SetActive(true);
            barrier2.SetActive(true);
            barrier3.SetActive(true);
        }
    }
    void StartBattle()
    {
        boss.GetComponent<Animator>().SetBool("CanMove", true);
    }
}
