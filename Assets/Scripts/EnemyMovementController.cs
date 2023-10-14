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

        foundPlayer = Physics.CheckSphere(transform.position, viewRange, whatIsPlayer);      //Flag that checks if player is within range/collides with the GameObject's sphere
        attackPlayer = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);      //Flag that checks if player is within attack range

        //Decision Tree
        if(!foundPlayer && !attackPlayer)
        {
            Patrolling();
        }
        if (foundPlayer && !attackPlayer)
        {
            Alert();
        }
        if (foundPlayer && attackPlayer)
        {
            Engage();
        } 

        if(!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            bool temp = AnimatorIsPlaying("dance");
            if (!hasPlayerPos && !foundPlayer && !attackPlayer && !temp)
            {
                onAPoint = false;
                GoToNextPoint();
            } else
            {
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

    private bool AnimatorIsPlaying(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, viewRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);
    }

}
