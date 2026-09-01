using UnityEngine;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    public float powerPercent = 0f;
    public float powerSpeed = 50f;
    public float rotateSpeed;

    public GameObject club;
    public GameObject ball;

    public void RotateClub(float direction)
    {
        transform.Rotate(Vector3.up * rotateSpeed * direction * Time.deltaTime);
    }

    public void UpdatePower(float newPower)
    {
        powerPercent += newPower;
        if (powerPercent > 1f)
        {
            powerPercent = 1f;
        }
        else if (powerPercent < 0f)
        {
            powerPercent = 0f;
        }
    }

    public void UpdateClubState(bool clubActiveState)
    {
        club.SetActive(clubActiveState);
    }

    public async Task SwingClub()
    {
        Debug.Log("Club is about to swing with " + powerPercent + " power level!");
        switch (powerPercent)
        {
            case < 0.18f:
                club.GetComponent<Animation>().Play("ClubSwing10");
                break;
            case >= 0.18f and < 0.34f:
                club.GetComponent<Animation>().Play("ClubSwing20");
                break;
            case >= 0.34f and < 0.51f:
                club.GetComponent<Animation>().Play("ClubSwing30");
                break;
            case >= 0.51f and < 0.68f:
                club.GetComponent<Animation>().Play("ClubSwing40");
                break;
            case >= 0.68f and < 0.84f:
                club.GetComponent<Animation>().Play("ClubSwing50");
                break;
            case >= 0.84f:
                club.GetComponent<Animation>().Play("ClubSwing60");
                break;
        }

        await Task.Delay(1000); // same as one second
        Debug.Log("Club has swung");
        await ball.GetComponent<Ball>().BallMovement(powerPercent, club);
    }

    public void UpdatePlayerPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
