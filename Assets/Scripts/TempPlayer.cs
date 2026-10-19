using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using WebSocketSharp;

public class TempPlayer : MonoBehaviourPunCallbacks
{
    private Transform tf;
    private PhotonView pv;
    private TempGameManager tempGM;
    private TextMeshProUGUI nametag;

    public float hp = 100f;
    public float speed = 5f;

    public float bulletPower = 10f;
    public float bulletSpeed = 6f;
    public float bulletCooldown = 60f;
    private float bulletTimer = 0f;
    private string ballColor = "";

    private float min;
    private float max;
    private float y;

    void Start()
    {
        tf = transform;
        pv = gameObject.GetComponent<PhotonView>();
        tempGM = GameObject.Find("TempGameManager").GetComponent<TempGameManager>();

        min = tempGM.boundsMin;
        max = tempGM.boundsMax;
        y = tempGM.y;

        if (!pv.IsMine)
        {
            Destroy(this);
        }

        nametag = transform.Find("Canvas/Player Nametag").GetComponent<TextMeshProUGUI>();
        nametag.text = pv.Owner.NickName;

        if (gameObject.name.Contains("red"))
            ballColor = "ball-red_var";
        else if (gameObject.name.Contains("green"))
            ballColor = "ball-green_var";
        else if (gameObject.name.Contains("blue"))
            ballColor = "ball-blue_var";
        else
            Debug.Log("Error: Player does not have a color");
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Control();

            // make sure everything is in bounds
            if (tf.position.x >= max)
                tf.position = new Vector3(max, y, tf.position.z);
            else if (tf.position.x <= min)
                tf.position = new Vector3(min, y, tf.position.z);

            if (tf.position.y != y)
                tf.position = new Vector3(tf.position.x, y, tf.position.z);

            if (tf.position.z >= max)
                tf.position = new Vector3(tf.position.x, y, max);
            else if (tf.position.z <= min)
                tf.position = new Vector3(tf.position.x, y, min);
        }
    }

    void Control()
    {
        bool[] direction = new bool[4];
        for (int i = 0; i < direction.Length; i++)
        {
            direction[i] = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            tf.position += Vector3.forward * speed * Time.deltaTime;
            direction[0] = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            tf.position += Vector3.left * speed * Time.deltaTime;
            direction[1] = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            tf.position += Vector3.back * speed * Time.deltaTime;
            direction[2] = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            tf.position += Vector3.right * speed * Time.deltaTime;
            direction[3] = true;
        }

        if (bulletTimer <= 0f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                bulletTimer = bulletCooldown;

                float x = 0;
                float z = 0;
                if (direction[0])
                    z += 0.1f;
                if (direction[1])
                    x -= 0.1f;
                if (direction[2])
                    z -= 0.1f;
                if (direction[3])
                    x += 0.1f;
                Vector3 offset = new Vector3(x, 0.9f, z);

                if (!ballColor.IsNullOrEmpty())
                {
                    GameObject bullet = PhotonNetwork.Instantiate(ballColor, tf.position + offset, Quaternion.identity);
                    bullet.GetComponent<TempBullet>().SetBulletDirection(x, z, bulletSpeed);
                }
            }
        }
        else
        {
            bulletTimer -= 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv != null && pv.IsMine)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                PhotonView bulletPv = other.gameObject.GetComponent<PhotonView>();
                if (bulletPv != null && !bulletPv.IsMine)
                {
                    hp -= bulletPower;

                    // string attackerName = bulletPv.Owner.NickName;
                    // string myName = pv.Owner.NickName;
                    // tempGM.CallRpcSendMessageToAll($"{attackerName} hit {myName}");

                    if (hp <= 0)
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                        Dead();
                    }
                }
            }
        }
    }

    public void Dead()
    {
        PhotonNetwork.Destroy(this.gameObject);
        tempGM.CallRpcLocalPlayerDead();
    }
}
