using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject startButton;
    public GameObject colorSelector;
    public GameObject loadingText;
    public Image backgroundImage;

    public void OnClickStart()
    {
        Debug.Log("ClickStart");
        LoadingUI();
        SceneManager.LoadScene("Test Course");
    }

    void LoadingUI()
    {
        startButton.SetActive(false);
        colorSelector.SetActive(false);
        backgroundImage.GetComponent<Image>().color = new Color32(227, 157, 197, 255);
        loadingText.SetActive(true);
    }
}
