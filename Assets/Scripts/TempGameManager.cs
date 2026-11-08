using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

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
            CheckGameOver();
        }
    }

    public void StartGame()
    {
        alivePlayerMap.Clear();
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            alivePlayerMap[player] = true;

        float x = Random.Range(boundsMin, boundsMax);
        float z = Random.Range(boundsMin, boundsMax);

        string flagColor = "flag-red_var";
        if (!flagColor.IsNullOrEmpty())
            PhotonNetwork.Instantiate(flagColor, new Vector3(x, y, z), Quaternion.identity);
        else
            Debug.Log("Error: Flag Color is empty");
    }

    [PunRPC]
    public void RpcPlayerDead(Player deadPlayer)
    {
        if (alivePlayerMap.ContainsKey(deadPlayer))
            alivePlayerMap[deadPlayer] = false;

        if (PhotonNetwork.IsMasterClient)
            CheckGameOver();
    }

    [PunRPC]
    public void RpcSendMessage(string message)
    {
        if (messageList.Count >= 8)
            messageList.RemoveAt(0);
        messageList.Add(message);
        UpdateMessage();
    }

    [PunRPC]
    public void RpcClearMessages()
    {
        messageList.Clear();
    }

    public void UpdateMessage()
    {
        if (messageText != null)
            messageText.text = string.Join("\n", messageList);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        alivePlayerMap[newPlayer] = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (alivePlayerMap.ContainsKey(otherPlayer))
            alivePlayerMap.Remove(otherPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            PhotonNetwork.LeaveRoom();

        if (PhotonNetwork.IsMasterClient)
            CheckGameOver();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void CheckGameOver()
    {
        List<Player> alivePlayers = new List<Player>();
        foreach (Player alivePlayer in alivePlayerMap.Keys)
            if (alivePlayerMap[alivePlayer])
                alivePlayers.Add(alivePlayer);

        if (alivePlayers.Count() == 1)
            StartCoroutine(LoadLevelForAll(alivePlayers[0].NickName, 5));
        else if (alivePlayers.Count() < 1)
            StartCoroutine(LoadLevelForAll("Error: No One", 5));
    }

    IEnumerator LoadLevelForAll(string lastPlayer, float delayInSeconds)
    {
        pv.RPC("RpcClearMessages", RpcTarget.All);
        pv.RPC("RpcSendMessage", RpcTarget.All, $"{lastPlayer} wins!");

        yield return new WaitForSeconds(delayInSeconds);

        PhotonNetwork.LoadLevel("Lobby");
    }
}
