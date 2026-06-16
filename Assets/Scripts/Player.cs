using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int speed;
    public bool canMove = false;
    public InputActionReference rotateRef;
    public InputActionReference heightRef;

    void Start()
    {
        
    }

    void FixedUpdate() // update is based on frame rate, fixed update is aligned with physics engine
    {
        if (canMove)
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime * rotateRef.action.ReadValue<int>());
            transform.Find("Club").Rotate(Vector3.forward * speed * Time.deltaTime * heightRef.action.ReadValue<int>());
        }
    }
}
