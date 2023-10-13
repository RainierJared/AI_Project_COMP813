using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderChecker : MonoBehaviour
{
    private EnemyMovementController obj;

    void Start()
    {
        obj = transform.parent.GetComponent<EnemyMovementController>();
    }
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER FOUND");
            obj.foundPlayer = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        obj.foundPlayer = false;
    }
}
