using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TempGameManager : MonoBehaviourPunCallbacks
{
    public Dictionary<Player, bool> alivePlayerMap = new Dictionary<Player, bool>();

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
            StartGame();
    }

    public void StartGame()
    {
        alivePlayerMap.Clear();
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            alivePlayerMap[player] = true;
        }

        // spawns the master client aka the first player aka the host
        float x = Random.Range(boundsMin, boundsMax);
        float z = Random.Range(boundsMin, boundsMax);
        PhotonNetwork.Instantiate("flag-red_var", new Vector3(x, y, z), Quaternion.identity);
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

    private void CheckGameOver()
    {
        Debug.Log("Check Game Over does nothing");
    }

    public void CallRpcSendMessageToAll(string message)
    {
        Debug.Log($"Call Rpc Send Message to All does nothing: {message}");
    }

    public void CallRpcLocalPlayerDead()
    {
        Debug.Log("Call Rpc Local Player Dead does nothing");
    }
}
