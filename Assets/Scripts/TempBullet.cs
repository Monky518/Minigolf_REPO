using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TempBullet : MonoBehaviourPunCallbacks
{
    private float x;
    private float z;
    private float speed;
    private bool start = false;

    public float bounds = 6f;

    void Update()
    {
        if (start)
            transform.position += new Vector3(x, 0, z) * speed * Time.deltaTime;

        if (transform.position.x > bounds || transform.position.x < -bounds || transform.position.z > bounds || transform.position.z < -bounds)
            PhotonNetwork.Destroy(this.gameObject);
    }

    public void SetBulletDirection(float x, float z, float speed)
    {
        while (x == 0 && z == 0)
        {
            x = Random.Range(-1, 2) * 0.1f;
            z = Random.Range(-1, 2) * 0.1f;
        }
        this.x = x * 10;
        this.z = z * 10;

        this.speed = speed;

        start = true;
    }
}
