using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform playerTarget;
    public Transform[] pointsArr;
    public Animator animator;
    public bool isWalking, onAPoint;
    public float viewRange, attackRange;
    private Vector3 lastKnownPos;

    //For 3 state AI

    //Patrolling
    public bool foundPlayer, attackPlayer;

    //Alert
    public bool hasPlayerPos;

    //Layer Meshes
    public LayerMask whatIsPlayer;

    //For Patrolling
    private int destPoint = 0;
    // Update is called once per frame

    void Start()
    {
        //Grabs components and GameObjects with sprcific tags
        enemy = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;

        //Console log tests and calling Patrolling state
        Debug.Log("Hi mom");
        Patrolling();

        //Setting Boolean states
        isWalking = true;
        foundPlayer = false;
        onAPoint = false;
        hasPlayerPos = false;
    }

    void Update()
    {
        //For Animator
        if (isWalking == true)
        {
            animator.SetTrigger("walk");
            animator.ResetTrigger("idle");
        }
        else
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("walk");
        }

        //Credit toe Dave / GameDevelopment in Unity to proviing these two lines for Booleans.
        foundPlayer = Physics.CheckSphere(transform.position, viewRange, whatIsPlayer);      //Flag that checks if player is within range/collides with the GameObject's sphere
        attackPlayer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);      //Flag that checks if player is within attack range
        
        //Decision Tree

        if(foundPlayer) {
            if(attackPlayer) {
                Engage();
            } else {
                Alert();
            }
        } else {
            if(!attackPlayer) {
                Patrolling();
            }
        }

        //Checks if the enemy's distance between them and the goal is close
        if(!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            bool temp = AnimatorIsPlaying("dance");
            //Checks if Booleans are false
            if (!hasPlayerPos && !foundPlayer && !attackPlayer && !temp)
            {
                //Calls this fucntion if they are
                onAPoint = false;
                GoToNextPoint();
            } else
            {
                //Starts the coroutine, forcing the AI wait at the player's last known location
                StartCoroutine(Wait());
            }

        }
    }


    //3 State Enemy AI
    void Patrolling()
    {
        animator.SetTrigger("walk");
        animator.ResetTrigger("dance");
        animator.ResetTrigger("idle");
        if (!onAPoint)
        {
            GoToNextPoint();
            onAPoint = true;
        }
    }   

    void Alert()
    {
        animator.SetTrigger("walk");
        animator.ResetTrigger("dance");
        animator.ResetTrigger("idle");
        GetPlayerPos();
        hasPlayerPos = true;
    }

    void Engage()
    {
        enemy.ResetPath();
        enemy.destination = transform.position;
        animator.SetTrigger("dance");
        animator.ResetTrigger("walk");
    }

    private void GoToNextPoint()
    {
            if (pointsArr.Length == 0)
            {
                return;
            }
            enemy.destination = pointsArr[destPoint].position;
            destPoint = (destPoint + 1) % pointsArr.Length;        //Modulo to cycle back to the start if end of array is reached.
    }

    private void GetPlayerPos()
    {
        if(hasPlayerPos)
        {
            lastKnownPos = playerTarget.position;
            enemy.destination = lastKnownPos;
        }
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        hasPlayerPos = false;
    }

    //Checks if Animator is playing an animation
    //
    private bool AnimatorIsPlaying(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    //Displays the spheres if Gizmos are enabled
    //To view, must be in play mode and click on "Enemies" on the left, and click on "gizmos" on the top bar, beneath the Game tab
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, viewRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);
    }

}
