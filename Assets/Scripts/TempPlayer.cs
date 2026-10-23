using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class TempPlayer : MonoBehaviourPunCallbacks
{
    private Transform tf;
    private PhotonView pv;
    private TempGameManager tempGM;

    [SerializeField] private GameObject bullet;
    [SerializeField] private TextMeshProUGUI nametag;
    [SerializeField] private Image healthBar;

    public float hp = 100f;
    [SerializeField] private float healthBarWidth = 80f;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float bulletPower = 10f;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletCooldown = 60f;
    private float bulletTimer = 0f;

    private float min;
    private float max;
    private float y;
    private AudioSource audioSource;
    private AudioClip impactSound;

    void Start()
    {
        tf = transform;
        pv = gameObject.GetComponent<PhotonView>();
        tempGM = GameObject.Find("TempGameManager").GetComponent<TempGameManager>();

        min = tempGM.boundsMin;
        max = tempGM.boundsMax;
        y = tempGM.y;
        audioSource = tempGM.audioSource;
        impactSound = tempGM.impactSound;

        if (!pv.IsMine)
        {
            Destroy(this);
        }

        nametag.text = pv.Owner.NickName;
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

                if (bullet != null)
                {
                    GameObject bullet = PhotonNetwork.Instantiate(this.bullet.name, tf.position + offset, Quaternion.identity);
                    bullet.GetComponent<TempBullet>().SetBulletDirection(x, z, bulletSpeed);
                }
            }
        }
        else
        {
            bulletTimer -= 1f;
        }
    }

    public void Dead()
    {
        PhotonNetwork.Destroy(this.gameObject);
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
                    HashTable table = new HashTable();
                    hp -= bulletPower;
                    table.Add("hp", hp);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(table);
                    audioSource.PlayOneShot(impactSound);

                    if (hp <= 0)
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                        Dead();
                    }
                }
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (targetPlayer == pv.Owner)
        {
            hp = (float)changedProps["hp"];
            healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarWidth * (hp / 100));
        }
    }
}
