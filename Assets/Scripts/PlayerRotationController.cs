using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    public Vector2 mouseTurn;

    // Update is called once per frame
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        mouseTurn.x += Input.GetAxis("Mouse X");
        mouseTurn.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-mouseTurn.y, mouseTurn.x, 0);
    }
}
