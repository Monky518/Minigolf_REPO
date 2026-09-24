using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput;
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

    public override void OnJoinedLobby()
    {
        Debug.Log("Successfully Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
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

    public void OnClickReturn()
    {
        LoadingUI("Leaving lobby...");
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Main Menu");
    }

    public void OnClickCreateRoom()
    {
        string roomName = GetRoomName();
        if (roomName != null)
            PhotonNetwork.CreateRoom(roomName);

        // if that room name already exists, show error message
    }

    public void OnClickJoinRoom()
    {
        string roomName = GetRoomName();
        if (roomName != null)
            PhotonNetwork.JoinRoom(roomName);

        // if that room does not exist, show error message
    }

    void LoadingUI(string text)
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
        else{
            Debug.Log("Invalid Room Name ( Nothing Written )");
            return null;
        }
    }
}
