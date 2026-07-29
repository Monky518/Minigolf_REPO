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

    private bool animationComplete = false;

    void FixedUpdate() {
        switch ( currentGameMode ) {
            case GameMode.MainMenu:
                break;
            case GameMode.PlayerPhase:
                PlayerPhase();
                break;
            case GameMode.BallMovement:
                BallStopCheck();
                CameraFollows();
                break;
            case GameMode.Results:
                break;
        }
    }

    void PlayerPhase() {
        // check if player is done
        if ( doneRef.action.ReadValue<float>() == 1f ) {
            StartAnimation();
            currentGameMode = GameMode.BallMovement;
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
                powerGauge.transform.Find( "Gauge" ).GetComponent<RectTransform>().sizeDelta = new Vector2( gaugeMaxWidth * powerPercent + gaugeMinWidth,gaugeHeight );
                arrow.GetComponent<RectTransform>().localPosition = new Vector3( 0,arrowStartPosition + (arrowRange * powerPercent),0 );
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
        // float normalForce = Physics.gravity.y * rb.mass;
        // float slopeAngleDeg = Vector3.Angle(other.contacts[0].normal, Vector3.up);
        // float frictionForce = coefficientOfFriction * normalForce * Mathf.Cos(slopeAngleDeg * Mathf.Deg2Rad);
        // rb.AddForce( frictionForce * rb.velocity.normalized );
        // rb.AddTorque( frictionForce * rb.angularVelocity.normalized );

        //ball moves!
        Rigidbody rbBall = player.transform.Find("Ball").GetComponent<Rigidbody>();
        rbBall.useGravity = true;
        // rbBall.linearVelocity = new Vector3( 0f,0f,(ballMaxSpeed - ballMinSpeed) * powerPercent + ballMinSpeed );
        rbBall.AddRelativeForce( new Vector3( 0f,0f,((ballMaxSpeed - ballMinSpeed) * powerPercent + ballMinSpeed) ),ForceMode.Impulse );
        Debug.Log( "Ball Speed: " + ((ballMaxSpeed - ballMinSpeed) * powerPercent + ballMinSpeed) );
        Debug.Log( "Power Percent: " + powerPercent );

        // check if ball is stopped
        

        // camera follows ball with offset

        // reset player ui
        // if not in hole, start player phase
        // else, start results phase
    }

    void BallStopCheck() {
        if ( animationComplete ) {
            Rigidbody rbBall = player.transform.Find("Ball").GetComponent<Rigidbody>();
            if (rbBall.linearVelocity.magnitude < 0.01f) {
                Debug.Log( "STOP THAT BALL!" );
            }
        }
    }

    void CameraFollows() {

    }
}
