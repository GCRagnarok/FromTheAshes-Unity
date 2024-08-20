using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_Phase2 : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 1.2f;

    public bool arrived1;
    public bool isFlipped;
    public bool departing;
    public bool iceFalse;

    Transform player;
    Rigidbody2D rb;
    Transform location1;
    Transform location2;
    Boss boss;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        location1 = GameObject.FindGameObjectWithTag("Location1").transform;
        location2 = GameObject.FindGameObjectWithTag("Location2").transform;
        boss = animator.GetComponent<Boss>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetComponent<BossSpawnIcicle>().spawned == true)
        {
            departing = true;
        }
        if(animator.GetComponent<BossSpawnIcicle>().spawned == false)
        {
            departing = false;
        }

        boss.LookAtPlayer();
        
        if(departing == false)
        {
            rb.position = location1.position;
        }
        if(departing == true)
        {
            rb.position = location2.position;
        }

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
        if (animator.GetBool("Phase2") == true && animator.GetBool("FinalStage") == false)
        {
            animator.GetComponent<BossSpawnIcicle>().canInstantiate = true;
            animator.GetComponent<EnemySpawnIcicle>().canInstantiate = true;
        }
        if (animator.GetBool("FinalStage") == true)
        {
            animator.GetComponent<BossSpawnProjectile>().canInstantiate = true;
            animator.GetComponent<BossSpawnIcicle>().canInstantiate = false;
            animator.GetComponent<EnemySpawnIcicle>().canInstantiate = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.GetComponent<BossSpawnIcicle>().canInstantiate = false;
        animator.GetComponent<EnemySpawnIcicle>().canInstantiate = false;
    }
}
