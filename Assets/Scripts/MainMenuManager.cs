using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class MainMenuManager: MonoBehaviourPunCallbacks
{
    public GameObject offlineButton;
    public GameObject onlineButton;
    public GameObject loadingText;
    public Image backgroundImage;
    
    public void OnClickStartOffline()
    {
        Debug.Log("Chosen Offline");
        SceneManager.LoadScene("Test Course");
    }
    
    public void OnClickStartOnline()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("ClickStart");
        LoadingUI();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        SceneManager.LoadScene("Lobby");
    }

    void LoadingUI()
    {
        offlineButton.SetActive(false);
        onlineButton.SetActive(false);
        backgroundImage.GetComponent<Image>().color = new Color32(227, 157, 197, 255);
        loadingText.SetActive(true);
    }
}
