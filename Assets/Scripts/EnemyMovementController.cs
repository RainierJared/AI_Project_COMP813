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

    //For 3 state AI

    //Patrolling
    public bool foundPlayer, attackPlayer;

    //Layer Meshes
    public LayerMask whatIsPlayer;

    //For Patrolling
    private int destPoint = 0;
    // Update is called once per frame

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
        isWalking = true;
        foundPlayer = false;
        onAPoint = false;
        Patrolling();
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
        if(foundPlayer && attackPlayer)
        {
            Engage();
        }

        //For if the enemy hasn't found the player and the path isn't pending
        if (!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            onAPoint = false;
            GoToNextPoint();
        }
    }


    //3 State Enemy AI
    void Patrolling()
    {
        GoToNextPoint();
    }   

    void Alert()
    {

    }

    void Engage()
    {

    }

    private void GoToNextPoint()
    {

        if (onAPoint == false)
        {
            if (pointsArr.Length == 0)
            {
                return;
            }
            onAPoint = true;
            enemy.destination = pointsArr[destPoint].position;
            destPoint = (destPoint + 1) % pointsArr.Length;        //Modulo to cycle back to the start if end of array is reached.
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, viewRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);
    }

}
