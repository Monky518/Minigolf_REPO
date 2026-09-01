using UnityEngine;
using System.Threading.Tasks;

public class Ball : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 10f;

    public async Task BallMovement(float powerPercept, GameObject club)
    {
        Rigidbody rbBall = GetComponent<Rigidbody>();

        rbBall.isKinematic = false;
        rbBall.useGravity = true;
        rbBall.AddRelativeForce(new Vector3(0f, 0f, ((maxSpeed - minSpeed) * powerPercept + minSpeed)), ForceMode.Impulse);

        await Task.Delay(3000);
    }

    public bool BallMovementCheck()
    {
        if (GetComponent<Rigidbody>().linearVelocity.magnitude < 0.01f)
            return false;
        else
            return true;
    }

    public void ResetBallPhysics()
    {
        Rigidbody rbBall = GetComponent<Rigidbody>();

        if (rbBall.linearVelocity.magnitude < 0.01f)
        {
            rbBall.useGravity = false;
            rbBall.isKinematic = true;
        }
    }

    public void ResetBallPositionAndRotation()
    {
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
