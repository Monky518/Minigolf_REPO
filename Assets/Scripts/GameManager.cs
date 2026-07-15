using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour {
    public enum GameMode {
        MainMenu,
        PlayerPhase,
        BallMovement,
        Results
    }
    public GameMode currentGameMode = GameMode.PlayerPhase; // temp until menu exists

    public GameObject player;
    public GameObject powerGauge;
    public GameObject arrow;

    public InputActionReference rotateRef;
    public InputActionReference gaugeRef;
    public InputActionReference doneRef;

    private float powerPercent = 0f;

    private float gaugeHeight = 40f;
    private float gaugeMinWidth = 5f;
    private float gaugeMaxWidth = 280f;
    private float gaugeSpeed = 50f;

    private float arrowStartPosition = -0.15f;
    private float arrowRange = 0.3f;

    void Update() {
        switch ( currentGameMode ) {
            case GameMode.MainMenu:
                break;
            case GameMode.PlayerPhase:
                PlayerPhase();
                break;
            case GameMode.BallMovement:
                BallMovement();
                break;
            case GameMode.Results:
                break;
        }
    }

    void PlayerPhase() {
        // check if player is done
        if ( doneRef.action.ReadValue<float>() == 1f )
            currentGameMode = GameMode.BallMovement;
        else {
            // check rotate movement
            if ( rotateRef.action.ReadValue<float>() != 0f )
                player.GetComponent<Player>().RotateClub( rotateRef.action.ReadValue<float>() );

            // check gauge movement
            if ( gaugeRef.action.ReadValue<float>() != 0f ) {
                // validate power level
                if ( powerPercent <= 1f && powerPercent >= 0f )
                    powerPercent += gaugeSpeed * (gaugeRef.action.ReadValue<float>() / 100) * Time.deltaTime;
                else if ( powerPercent > 1f )
                    powerPercent = 1f;
                else if ( powerPercent < 0f )
                    powerPercent = 0f;

                // update UI
                powerGauge.transform.Find( "Gauge" ).GetComponent<RectTransform>().sizeDelta = new Vector2( gaugeMaxWidth * powerPercent + gaugeMinWidth,gaugeHeight );
                arrow.GetComponent<RectTransform>().localPosition = new Vector3( 0,arrowStartPosition + (arrowRange * powerPercent),0 );
            }
        }
    }

    void BallMovement() {
        // get rid of player ui
        powerGauge.SetActive( false );
        arrow.SetActive( false );

        // start animation
        switch ( powerPercent ) {
            case < 0.18f:
                player.transform.Find( "Club" ).GetComponent<Animation>().Play( "ClubSwing10" );
                break;
            case >= 0.18f and < 0.34f:
                player.transform.Find( "Club" ).GetComponent<Animation>().Play( "ClubSwing20" );
                break;
            case >= 0.34f and < 0.51f:
                player.transform.Find( "Club" ).GetComponent<Animation>().Play( "ClubSwing30" );
                break;
            case >= 0.51f and < 0.68f:
                player.transform.Find( "Club" ).GetComponent<Animation>().Play( "ClubSwing40" );
                break;
            case >= 0.68f and < 0.84f:
                player.transform.Find( "Club" ).GetComponent<Animation>().Play( "ClubSwing50" );
                break;
            case >= 0.84f:
                player.transform.Find( "Club" ).GetComponent<Animation>().Play( "ClubSwing60" );
                break;
        }

        // wait for animation
        StartCoroutine( WaitForAnimation() );

        // ball moves and camera follows
        // reset player ui
        // if not in hole, start player phase
        // else, start results phase
    }

    IEnumerator WaitForAnimation() {
        yield return new WaitForSeconds( 1f );
        Debug.Log( "Animation Complete!" );
    }
}
