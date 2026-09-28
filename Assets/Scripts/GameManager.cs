using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PlayerPhase,
        ClubSwing,
        BallPhase,
        Calculating
    }
    public GameState currentState = GameState.PlayerPhase;

    public GameObject player;

    public GameObject powerGauge;
    public GameObject arrow;

    public InputActionReference rotateRef;
    public InputActionReference gaugeRef;
    public InputActionReference doneRef;

    private float gaugeHeight = 40f;
    private float gaugeMinWidth = 5f;
    private float gaugeMaxWidth = 280f;

    private float arrowStartPosition = -0.15f;
    private float arrowRange = 0.3f;

    void FixedUpdate()
    {
        if (currentState == GameState.PlayerPhase)
            PlayerPhase();
        else if (currentState == GameState.BallPhase)
            BallPhase();
    }

    async void PlayerPhase()
    {
        if (doneRef.action.ReadValue<float>() == 1f)
        {
            currentState = GameState.ClubSwing;
            UpdateUIState(false);
            await player.GetComponent<GolfPlayer>().SwingClub();
            currentState = GameState.BallPhase;
        }
        else
        {
            if (rotateRef.action.ReadValue<float>() != 0f)
                player.GetComponent<GolfPlayer>().RotateClub(rotateRef.action.ReadValue<float>());

            if (gaugeRef.action.ReadValue<float>() != 0f)
            {
                player.GetComponent<GolfPlayer>().UpdatePower(gaugeRef.action.ReadValue<float>() / 100);
                MoveUI(player.GetComponent<GolfPlayer>().powerPercent);
            }
        }
    }

    void BallPhase()
    {
        GameObject ball = player.GetComponent<GolfPlayer>().ball;

        if (!ball.GetComponent<Ball>().BallMovementCheck())
        {
            ball.GetComponent<Ball>().ResetBallPhysics();
            currentState = GameState.Calculating;

            // if in hole
            // do thing
            // if out of bounds
            // do thing
            // else
            // play continues
            player.GetComponent<GolfPlayer>().UpdatePlayerPosition(ball.transform.position);
            ball.GetComponent<Ball>().ResetBallPositionAndRotation();
            ResetPlayerPhase();
        }
    }

    void ResetPlayerPhase()
    {
        UpdateUIState(true);
        player.GetComponent<GolfPlayer>().UpdateClubState(true);
        player.GetComponent<GolfPlayer>().UpdatePower(-1f);
        MoveUI(player.GetComponent<GolfPlayer>().powerPercent);
        currentState = GameState.PlayerPhase;
    }

    void UpdateUIState(bool uiState)
    {
        powerGauge.SetActive(uiState);
        arrow.SetActive(uiState);
    }

    void MoveUI(float powerPercent)
    {
        powerGauge.transform.Find("Gauge").GetComponent<RectTransform>().sizeDelta = new Vector2(gaugeMaxWidth * powerPercent + gaugeMinWidth, gaugeHeight);
        arrow.GetComponent<RectTransform>().localPosition = new Vector3(0, arrowStartPosition + (arrowRange * powerPercent), 0);
    }
}
