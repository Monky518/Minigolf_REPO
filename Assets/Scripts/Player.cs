using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum GameMode
    {
        MainMenu,
        PlayerMovement,
        BallMovement,
        Results
    }
    public GameMode currentGameMode= GameMode.PlayerMovement;
    
    public float speed;
    public float heightMin;
    public float heightMax;

    public InputActionReference rotateRef;
    public InputActionReference heightRef;

    private float rotate;
    private float height;
    private GameObject club;

    void Start()
    {
        club = transform.Find("Club").gameObject;
    }

    private void Update() // based on frame rate
    {
        rotate = rotateRef.action.ReadValue<float>();
        height = heightRef.action.ReadValue<float>();
    }

    void FixedUpdate() // aligned with physics engine
    {
        if (currentGameMode == GameMode.PlayerMovement)
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime * rotate);

            Debug.Log("Club Rotation: " + club.transform.eulerAngles);

            if (club.transform.eulerAngles.z >= heightMin && club.transform.eulerAngles.z <= heightMax)
                club.transform.Rotate(Vector3.forward * speed * Time.deltaTime * height);
            else if (club.transform.eulerAngles.z < heightMin)
                club.transform.eulerAngles = new Vector3(club.transform.eulerAngles.x, club.transform.eulerAngles.y, heightMin);
            else if (club.transform.eulerAngles.z > heightMax)
                club.transform.eulerAngles = new Vector3(club.transform.eulerAngles.x, club.transform.eulerAngles.y, heightMax);
        }
    }
}
