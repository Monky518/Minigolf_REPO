using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using WebSocketSharp;
using TMPro;

public class TempGameManager : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    public Dictionary<Player, bool> alivePlayerMap = new Dictionary<Player, bool>();

    public AudioSource audioSource;
    public AudioClip impactSound;

    private List<string> messageList = new List<string>();
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI roomNameText;

    public float boundsMin = -4.8f;
    public float boundsMax = 4.8f;
    public float y = 0.06f;

    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("Lobby");
            return;
        }
        else
        {
            pv = gameObject.GetComponent<PhotonView>();
            roomNameText.text = $"{PhotonNetwork.CurrentRoom.Name} Room";
            StartGame();
        }
    }

    public void StartGame()
    {
        alivePlayerMap.Clear();
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            alivePlayerMap[player] = true;
        }

        float x = Random.Range(boundsMin, boundsMax);
        float z = Random.Range(boundsMin, boundsMax);

        string flagColor = "flag-red_var";
        if (!flagColor.IsNullOrEmpty())
            PhotonNetwork.Instantiate(flagColor, new Vector3(x, y, z), Quaternion.identity);
        else
            Debug.Log("Error: Flag Color is empty");
    }

    [PunRPC]
    public void RpcSendMessage(string message, PhotonMessageInfo info)
    {
        if (messageList.Count >= 8)
            messageList.RemoveAt(0);
        messageList.Add(message);
        UpdateMessage();
    }

    public void CallRpcSendMessageToAll(string message)
    {
        pv.RPC("RpcSendMessage", RpcTarget.All, message);
    }

    public void UpdateMessage()
    {
        if (messageText != null)
            messageText.text = string.Join("\n", messageList);
    }

    private void CheckGameOver()
    {
        Debug.Log("Check Game Over does nothing");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        alivePlayerMap[newPlayer] = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (alivePlayerMap.ContainsKey(otherPlayer))
        {
            alivePlayerMap.Remove(otherPlayer);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            CheckGameOver();
        }
    }
}
