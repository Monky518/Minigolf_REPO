using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
            roomName.text = "Current Room: " + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayers();
        }
    }

    private void UpdatePlayers()
    {
        // Instead of a player list, I want the names to be above the their club and ball color.
        //      Later, they can choose their own color. It is based on join order right now.

        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < 3; i++)
        {
            if (players.Length > i)
            {
                if (players[i].NickName != "")
                    colorTexts[i].text = players[i].NickName;
                else
                {
                    Debug.Log("Player " + i + " does not have a nickname");
                    colorTexts[i].text = "Player " + i;
                }
            }
            else
                colorTexts[i].text = "";
        }

        startButton.interactable = PhotonNetwork.IsMasterClient;
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

    public void OnClickStartGame()
    {
        LoadingUI("Loading Test Course...");
        SceneManager.LoadScene("Test Course");
    }

    public void OnClickRoomSettings()
    {
        // allow pop up for room settings
        Debug.Log("Clicked Room Settings, which is not implemented yet");
    }

    public void OnClickLeaveRoom()
    {
        LoadingUI("Leaving room...");
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Main Menu");
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
}
