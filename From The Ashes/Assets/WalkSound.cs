using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    private float nextStepTime = 0;
    private float stepTime;
    public float startStepTime;
    public float stepCooldownTime;

    public bool canStep;

    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        stepTime = startStepTime;
        canStep = true;

    }
    void Update()
    {
        if (Time.time > nextStepTime)
        {
            if (stepTime <= 0)
            {
                stepTime = startStepTime;
            }
            else
            {
                stepTime -= Time.deltaTime;
            }
            if (Input.GetAxis("Horizontal") != 0 && GetComponent<PlayerController>().isGrounded == true && canStep == true)
            {
                audioSource.Play();
                startStepTime = Time.time + stepCooldownTime;
            }
        }
    }
}

