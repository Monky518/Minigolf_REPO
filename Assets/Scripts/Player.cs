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
    public GameMode currentGameMode = GameMode.PlayerMovement;
    
    public float rotateSpeed;
    public float gaugeSpeed;

    public InputActionReference rotateRef;
    public InputActionReference gaugeRef;
    public InputActionReference endPlayerMovementRef;

    public RectTransform gauge;

    private GameObject club;
    private float powerPercent = 0f;

    private float gaugeHeight = 40f;
    private float gaugeMinWidth = 5f;
    private float gaugeMaxWidth = 280f;

    void Start()
    {
        club = transform.Find("Club").gameObject;
    }

    private void Update() // based on frame rate
    {
        if (currentGameMode == GameMode.PlayerMovement)
        {
            if (!endPlayerMovementRef.action.ReadValue<Boolean>())
            {
                transform.Rotate(Vector3.up * rotateSpeed * rotateRef.action.ReadValue<float>() * Time.deltaTime);

                if (powerPercent <= 1f && powerPercent >= 0f)
                    powerPercent += gaugeSpeed * (gaugeRef.action.ReadValue<float>() / 100) * Time.deltaTime;
                else if (powerPercent > 1f)
                    powerPercent = 1f;
                else if (powerPercent < 0f)
                    powerPercent = 0f;

                   gauge.sizeDelta = new Vector2(gaugeMaxWidth * powerPercent + gaugeMinWidth, gaugeHeight);
            } else
            {
                currentGameMode = GameMode.BallMovement;
            }
        } else if (currentGameMode == GameMode.BallMovement)
        {
            // get rid of player ui
            // start animation
            // ball moves and camera follows
            // reset player ui
            // if not in hole, start player phase
            // else, start results phase
        }
    }

    void FixedUpdate() // aligned with physics engine
    {

    }
}
