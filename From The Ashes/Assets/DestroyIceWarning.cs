using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIceWarning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 0.5f);
    }

    // Update is called once per frame
    void Destroy()
    {
        Destroy(gameObject);
    }
}
