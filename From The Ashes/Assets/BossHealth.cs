using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 300;
    public int currentHealth = 300;

    public bool isInvulnerable = false;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("DamageEnemy");
        GetComponent<Animator>().SetTrigger("Hit");

        if (currentHealth <= 200)
        {
            GetComponent<Animator>().SetBool("SecondStage", true);
        }

        if (currentHealth <= 100)
        {
            GetComponent<Animator>().SetBool("FinalStage", true);
        }

        if (currentHealth <= 0)
        {
            GetComponent<Animator>().SetBool("IsDead", true);
            FindObjectOfType<AudioManager>().Play("BossDeath");
            Invoke("Die", 0.5f);
        }
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("WinScreen");
        Destroy(GameObject.Find("BGM"));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("PhoenixProjectile"))
        {
            print("hit");
            TakeDamage(25);
        }
    }

}

