using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerBody;
    public float wSpeed, wbSpeed, olwSpeed, rnSpeed, roSpeed;
    public bool isWalking;
    public Transform playerTransform;
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerBody.velocity = transform.forward * wSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerBody.velocity = -transform.forward * wbSpeed * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Walking
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("walk");
            playerAnim.ResetTrigger("idle");
            isWalking = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
            isWalking = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("walkBack");
            playerAnim.ResetTrigger("idle");
            isWalking = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("walkBack");
            playerAnim.SetTrigger("idle");
            isWalking = true;
        }

        //Rotation
        if (Input.GetKey(KeyCode.A))
        {
            playerTransform.Rotate(0, -roSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerTransform.Rotate(0, roSpeed * Time.deltaTime, 0);
        }

        //Sprinting
        if (isWalking == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Increasing velocity
                wSpeed = wSpeed * rnSpeed;
                playerAnim.SetTrigger("run");
                playerAnim.ResetTrigger("walk");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //Increasing velocity
                wSpeed = olwSpeed;
                playerAnim.ResetTrigger("run");
                playerAnim.SetTrigger("walk");
            }
        }
    }
}
