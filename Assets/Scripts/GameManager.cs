using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour {
    private bool playerCanMove = true;    // temp until main menu exists
    private bool ballIsMoving = false;

    public GameObject player;
    public GameObject powerGauge;
    public GameObject arrow;

    public InputActionReference rotateRef;
    public InputActionReference gaugeRef;
    public InputActionReference doneRef;

    public float rotateSpeed = 10f;

    private float gaugeHeight = 40f;
    private float gaugeMinWidth = 5f;
    private float gaugeMaxWidth = 280f;
    private float gaugeSpeed = 50f;

    private float arrowStartPosition = -0.15f;
    private float arrowRange = 0.3f;

    private float powerPercent = 0f;
    public float ballMinSpeed = 1f;
    public float ballMaxSpeed = 10f;

    void FixedUpdate() {
        if ( playerCanMove ) {
            PlayerPhase();
        } else if ( ballIsMoving ) {
            BallStopCheck();
        }
    }

    void PlayerPhase() {
        // check if player is done
        if ( doneRef.action.ReadValue<float>() == 1f ) {
            playerCanMove = false;
            StartAnimation();
        } else {
            // check rotate movement
            if ( rotateRef.action.ReadValue<float>() != 0f )
                player.transform.Rotate( Vector3.up * rotateSpeed * rotateRef.action.ReadValue<float>() * Time.deltaTime );

            // check gauge movement
            if ( gaugeRef.action.ReadValue<float>() != 0f ) {
                if ( powerPercent <= 1f && powerPercent >= 0f )
                    powerPercent += gaugeSpeed * (gaugeRef.action.ReadValue<float>() / 100) * Time.deltaTime;

                // validate power level
                if ( powerPercent > 1f )
                    powerPercent = 1f;
                else if ( powerPercent < 0f )
                    powerPercent = 0f;

                // update UI
                UpdateUI();
            }
        }
    }

    void StartAnimation() {
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
    }

    IEnumerator WaitForAnimation() {
        yield return new WaitForSeconds( 1f );
        BallMovement();
    }

    void BallMovement() {
        // ball moves!
        Rigidbody rbBall = player.transform.Find( "Ball" ).GetComponent<Rigidbody>();
        rbBall.useGravity = true;
        rbBall.AddRelativeForce( new Vector3( 0f,0f,((ballMaxSpeed - ballMinSpeed) * powerPercent + ballMinSpeed) ),ForceMode.Impulse );

        // get rid of club
        Debug.Log( "Club is going away!" );
        player.transform.Find( "Club" ).gameObject.SetActive( false );

        StartCoroutine( WaitForStartOfBallMovement() );
    }

    IEnumerator WaitForStartOfBallMovement() {
        yield return new WaitForSeconds( 1f );
        ballIsMoving = true;
    }

    void BallStopCheck() {
        Rigidbody rbBall = player.transform.Find( "Ball" ).GetComponent<Rigidbody>();
        Debug.Log( "Checking if ball has stopped" );
        Debug.Log( "Ball Velocity: " + rbBall.linearVelocity.magnitude );
        if ( rbBall.linearVelocity.magnitude < 0.01f ) {
            Debug.Log( "Ball has stopped!" );
            ballIsMoving = false;
            // if in hole

            // else if out of course
            // reset ball and camera* to player position
            // RepositionPlayer( player.transform.position );

            // else
            // reposition player and ball
            RepositionPlayer( player.transform.Find( "Ball" ).transform.position );

            // reset player phase and UI
            ResetPlayerPhase();
        } else {
            Debug.Log( "Ball is moving!" );
        }
    }

    void RepositionPlayer( Vector3 playerPosition ) {
        // set player position
        player.transform.position = playerPosition;
        // update ball to match
        player.transform.Find( "Ball" ).transform.localPosition = new Vector3( 0,0,0 );
    }

    void ResetPlayerPhase() {
        powerGauge.SetActive( true );
        arrow.SetActive( true );
        Debug.Log( "Club is coming back!" );
        player.transform.Find( "Club" ).gameObject.SetActive( true );

        powerPercent = 0f;
        UpdateUI();

        playerCanMove = true;
    }

    void UpdateUI() {
        powerGauge.transform.Find( "Gauge" ).GetComponent<RectTransform>().sizeDelta = new Vector2( gaugeMaxWidth * powerPercent + gaugeMinWidth,gaugeHeight );
        arrow.GetComponent<RectTransform>().localPosition = new Vector3( 0,arrowStartPosition + (arrowRange * powerPercent),0 );
    }
}
