using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerBody;
    public Transform playerTransform;

    //Player Movement
    public UnityEngine.Vector3 playerMovement;
    public float speed;
    public bool isWalking;

    void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //Movement
        float playerVertical = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        float playerHorizontal = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;

        UnityEngine.Vector3 forward = Camera.main.transform.forward;
        UnityEngine.Vector3 right = Camera.main.transform.right;
        forward.y= 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        UnityEngine.Vector3 forwardRelative = playerVertical * forward;
        UnityEngine.Vector3 rightRelative= playerHorizontal * right;

        playerMovement = forwardRelative + rightRelative ;
        
        transform.Translate(playerMovement, Space.World);

        //Animations
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

        if (isWalking == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Increasing velocity
                playerAnim.SetTrigger("run");
                playerAnim.ResetTrigger("walk");
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //Increasing velocity
                playerAnim.ResetTrigger("run");
                playerAnim.SetTrigger("idle");
            }
        }
    }
}
