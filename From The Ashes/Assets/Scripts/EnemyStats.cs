using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStats : MonoBehaviour
{
    public Animator animator;
    public GameObject enemy;
    public GameObject projectile;
    public GameObject player;
    public GameObject healthPickup;
    public GameObject[] enemies;
    AudioSource audioSource;

    public bool isDead;
    public bool died;

    public int maxHealth = 100;
    public int currentHealth = 100;
    public int attackDamage;
    public int spawnValue;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (died == true && isDead == false)
        {
            OnDestroy();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("DamageEnemy");

        animator.SetTrigger("Hit");
        

        if(currentHealth <= 0 && gameObject.tag == ("Enemies"))
        {
            player.GetComponent<PlayerController>().enemiesDefeated = player.GetComponent<PlayerController>().enemiesDefeated +1;
            enemy.GetComponent<AIPath>().canSearch = false;
            enemy.GetComponent<AIPath>().canMove = false;
            Die();
            
        }
        if(currentHealth <= 0 && gameObject.tag == ("Fire"))
        {
            player.GetComponent<PlayerController>().enemiesDefeated = player.GetComponent<PlayerController>().enemiesDefeated + 1;
            enemy.GetComponent<AIPath>().canSearch = false;
            enemy.GetComponent<AIPath>().canMove = false;
            enemy.GetComponent<EnemySpawnProjectiles>().canInstantiate = false;
            projectile.GetComponent<EnemyFireProjectile>().canFire = false;
            Die();
        }
        if (currentHealth <= 0 && gameObject.tag == ("Ice"))
        {
            player.GetComponent<PlayerController>().enemiesDefeated = player.GetComponent<PlayerController>().enemiesDefeated + 1;
            enemy.GetComponent<AIPath>().canSearch = false;
            enemy.GetComponent<AIPath>().canMove = false;
            enemy.GetComponent<EnemySpawnIcicle>().canInstantiate = false;
            Die();
        }
    }

    void Die()
    {
        if (gameObject.tag == ("Fire"))
        {
            GetComponent<Collider2D>().enabled = false;
            audioSource.Play();
            isDead = true;
            died = true;
            Debug.Log("Enemy died!");
            animator.SetBool("IsDead", true);
            Invoke("OnDestroy", 2.0f);
        }
        if (gameObject.tag == ("Ice"))
        {
            GetComponent<Collider2D>().enabled = false;
            audioSource.Play();
            isDead = true;
            died = true;
            Debug.Log("Enemy died!");
            animator.SetBool("IsDead", true);
            Invoke("OnDestroy", 2.0f);

        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            audioSource.Play();
            isDead = true;
            died = true;
            Debug.Log("Enemy died!");
            animator.SetBool("IsDead", true);
            Invoke("OnDestroy", 2.0f);
        }
    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == ("Player"))
        {
            print("hit");
            player.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
        if(collision.gameObject.tag == ("PhoenixProjectile"))
        {
            print("hit");
            TakeDamage(25);
        }
    }
}
