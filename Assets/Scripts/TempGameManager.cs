using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using WebSocketSharp;

public class TempGameManager : MonoBehaviourPunCallbacks
{
    public Dictionary<Player, bool> alivePlayerMap = new Dictionary<Player, bool>();

    public AudioSource audioSource;
    public AudioClip impactSound;
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

        float x = Random.Range(boundsMin, boundsMax);
        float z = Random.Range(boundsMin, boundsMax);

        // this was taken from Google and modified
        // it does not work :(
        string[] flagColors = new string[3] { "flag-red_var", "flag-green_var", "flag-blue_var" };
        string flagColor = "flag-red_var"; // set red as default until actual color changes are implemented
        for (int i = 1; i <= flagColors.Length; i++)
        {
            if (GetPlayerIndex(PhotonNetwork.LocalPlayer) % 3 == i)
                flagColor = flagColors[i - 1];
        }

        if (!flagColor.IsNullOrEmpty())
            PhotonNetwork.Instantiate(flagColor, new Vector3(x, y, z), Quaternion.identity);
        else
            Debug.Log("Error: Flag Color is empty");
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

    // this was taken from Google
    private int GetPlayerIndex(Player player)
    {
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].ActorNumber == player.ActorNumber)
                return i;
        }
        return 0;
    }
}
