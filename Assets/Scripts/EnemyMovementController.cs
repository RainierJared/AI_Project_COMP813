using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Target;
    public Animator animator;
    public bool isWalking;
    // Update is called once per frame

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
    }

    void FixedUpdate()
    {
        if(isWalking == true)
        {
            animator.SetTrigger("walk");
            animator.ResetTrigger("idle");
        } else
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("walk");
        }
    }

    void Update()
    {
        if (Target != null)
        {
            enemy.destination = Target.position;
            if(enemy.velocity.magnitude > 0f)
            {
                isWalking = true;
            } else
            {
                isWalking = false;
            }
        }

        }    
}
