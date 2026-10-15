using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput;
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI roomListText;
    public GameObject loadingText;
    public Image backgroundImage;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            LoadingUI("Loading Main Menu...");
            SceneManager.LoadScene("Main Menu");
        }
        else
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        LoadingUI($"Loading {GetRoomName()}...");
        SceneManager.LoadScene("Room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StringBuilder sb = new StringBuilder();
        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount > 0)
                sb.AppendLine($"   ( {room.PlayerCount} / 3 Players ) - {room.Name}");
            else
                sb.AppendLine("No Rooms Available");
            roomListText.text = sb.ToString();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnClickReturn()
    {
        LoadingUI("Leaving lobby...");
        PhotonNetwork.Disconnect();
    }

    public void OnClickCreateRoom()
    {
        string playerName = GetPlayerName();
        if (!string.IsNullOrEmpty(playerName))
            PhotonNetwork.LocalPlayer.NickName = playerName;

        string roomName = GetRoomName();
        if (!string.IsNullOrEmpty(roomName))
            PhotonNetwork.CreateRoom(roomName);

        // if that room name already exists, show error message
    }

    public void OnClickJoinRoom()
    {
        string playerName = GetPlayerName();
        if (!string.IsNullOrEmpty(playerName))
            PhotonNetwork.LocalPlayer.NickName = playerName;

        string roomName = GetRoomName();
        if (!string.IsNullOrEmpty(roomName))
            PhotonNetwork.JoinRoom(roomName);

        // if that room does not exist, show error message

        // if room is full, show error message
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

    public string GetRoomName()
    {
        string roomName = roomNameInput.text;
        roomName.Trim();
        if (roomName.Length > 0)
            return roomName;
        else
        {
            Debug.Log("Invalid Room Name ( Nothing Written )");
            return null;
        }
    }

    public string GetPlayerName()
    {
        string playerName = playerNameInput.text;
        return playerName.Trim();
    }
}
