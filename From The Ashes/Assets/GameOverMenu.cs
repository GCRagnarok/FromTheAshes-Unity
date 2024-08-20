using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject player;

    public Animator transitionAnim;

    public void PlayGame()
    {
        transitionAnim.SetTrigger("end");
        SceneManager.LoadScene("Sanctuary1Left");
        PlayerPrefs.DeleteAll();
        player.GetComponent<PlayerController>().currentHealth = player.GetComponent<PlayerController>().maxHealth;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
