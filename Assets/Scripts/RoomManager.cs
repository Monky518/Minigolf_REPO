using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using WebSocketSharp;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject loadingText;
    public Image backgroundImage;
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI[] colorTexts;
    public Button startButton;

    public void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            LoadingUI("Loading Main Menu...");
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            roomName.text = $"Current Room: {PhotonNetwork.CurrentRoom.Name}";
            UpdatePlayers();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdatePlayers();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    private void UpdatePlayers()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < 3; i++)
        {
            if (players.Length > i)
            {
                Debug.Log($"Player {i} Name: {players[i].NickName}");

                if (players[i].NickName.IsNullOrEmpty())
                {
                    players[i].NickName = $"Player {i + 1}";
                }
                colorTexts[i].text = players[i].NickName;
            }
            else
                colorTexts[i].text = "";
        }

        startButton.interactable = PhotonNetwork.IsMasterClient;
    }

    public void OnClickStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LoadingUI("Loading Game...");
            PhotonNetwork.LoadLevel("Temp Game Scene");
        }
    }

    public void OnClickRoomSettings()
    {
        // allow pop up for room settings
        // should this be master client only?
        Debug.Log("Clicked Room Settings, which is not implemented yet");
    }

    public void OnClickLeaveRoom()
    {
        LoadingUI("Leaving room...");
        PhotonNetwork.LeaveRoom();
    }

    public void LoadingUI(string text)
    {
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject element in uiElements)
            element.SetActive(false);
        backgroundImage.GetComponent<Image>().color = new Color32(227, 157, 197, 255);
        loadingText.GetComponent<TMP_Text>().text = text;
        loadingText.SetActive(true);
    }
}
