using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TempPlayer : MonoBehaviourPunCallbacks
{
    //public GameObject playerPrefab;

    //public float min = 0.49f;
    //public float max = 0.49f;
    //public float y = 0.06f;

    //void OnEnable()
    //{
    //    if (PhotonNetwork.InRoom)
    //        SpawnPlayer();
    //}

    //public override void OnJoinedRoom()
    //{
    //    SpawnPlayer();
    //}

    //private void SpawnPlayer()
    //{
    //    if (playerPrefab == null)
    //    {
    //        return;
    //    }

    //    float x = Random.Range(min, max);
    //    float z = Random.Range(min, max);
    //    Vector3 spawnPosition = new Vector3(x, y, z);

    //    PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    //}

    private Transform tf;
    public PhotonView pv;
    private TempGameManager tempGM;
    private TextMeshProUGUI nametag;

    public float hp = 100f;
    public float speed;
    public float bulletPower;
    public float bulletSpeed;
    public string bulletName;

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
        nametag.text = PhotonNetwork.LocalPlayer.NickName;
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
        if (Input.GetKey(KeyCode.W))
        {
            tf.position += Vector3.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            tf.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            tf.position += Vector3.back * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            tf.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //Vector3 offset = new Vector3(0.1f, 0, 0);
            //GameObject bullet = PhotonNetwork.Instantiate(bulletName, tf.position + offset, Quaternion.identity);
            // add bullet movement here ( goal: without rigidbody )
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (pv != null && pv.IsMine)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                PhotonView bulletPv = other.gameObject.GetComponent<PhotonView>();
                if (bulletPv != null && !bulletPv.IsMine)
                {
                    hp -= bulletPower;

                    string attackerName = bulletPv.Owner.NickName;
                    string myName = pv.Owner.NickName;
                    tempGM.CallRpcSendMessageToAll($"{attackerName} hit {myName}");

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
