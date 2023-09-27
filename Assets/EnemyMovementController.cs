using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Target;
    public Animator animator;
    // Update is called once per frame

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Target != null)
        {
            animator.SetTrigger("walk");
            enemy.destination = Target.position;
        }    
    }
}
