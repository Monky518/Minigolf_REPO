using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject loadingText;
    public Image backgroundImage;

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
