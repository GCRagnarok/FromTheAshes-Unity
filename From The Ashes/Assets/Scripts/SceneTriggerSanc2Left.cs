using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTriggerSanc2Left : MonoBehaviour
{
    public Animator transitionAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            StartCoroutine(SceneTransition());
        }
    }

    IEnumerator SceneTransition()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Sanctuary2Left");
        Destroy(GameObject.Find("BGM"));
    }
}