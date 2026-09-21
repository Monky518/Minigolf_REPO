using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class MainMenuManager: MonoBehaviourPunCallbacks
{
    public GameObject loadingText;
    public Image backgroundImage;
    
    public void OnClickStartOffline()
    {
        LoadingUI("Loading Test Course...");
        SceneManager.LoadScene("Test Course");
    }
    
    public void OnClickStartOnline()
    {
        PhotonNetwork.ConnectUsingSettings();
        LoadingUI("Loading Lobby...");
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
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
