using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public bool canMove = false;
    public InputActionReference rotateRef;
    public InputActionReference heightRef;

    private float rotate;
    private float height;

    void Start()
    {
        
    }

    private void Update() // based on frame rate
    {
        rotate = rotateRef.action.ReadValue<float>();
        height = heightRef.action.ReadValue<float>();
        Debug.Log("Rotate: " +  rotate);
        Debug.Log("Height: " +  height);
    }

    void FixedUpdate() // aligned with physics engine
    {
        Debug.Log("Fixed Update Works");
        if (canMove)
        {
            Debug.Log("Player Can Move");
            transform.Rotate(Vector3.up * speed * Time.deltaTime * rotate);
            transform.Find("Club").Rotate(Vector3.forward * speed * Time.deltaTime * height);
        }
    }
}
