using UnityEngine;

public class Player : MonoBehaviour {
    public float rotateSpeed;

    public void RotateClub( float direction ) {
        transform.Rotate( Vector3.up * rotateSpeed * direction * Time.deltaTime );
    }
}
