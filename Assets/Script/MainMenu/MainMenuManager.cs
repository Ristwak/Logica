using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject loadingPanel;

    void Start()
    {
        menuPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    public void OnStartGame()
    {
        // Load first level or scene
        Debug.Log("Starting Game");
        menuPanel.SetActive(false);
        SceneManager.LoadScene("HomeScene");
        loadingPanel.SetActive(true);
        menuPanel.SetActive(false);
        SoundManager.Instance.PlaySound("click");
    }

    public void OnLearn() {
        // Open tutorial panel
        Debug.Log("Learn panel opened.");
        loadingPanel.SetActive(true);
        menuPanel.SetActive(false);
        SoundManager.Instance.PlaySound("click");
    }

    public void OnProgress() {
        // Open progress tracking panel
        Debug.Log("Progress panel opened.");
        SoundManager.Instance.PlaySound("click");
    }

    public void OnSettings() {
        // Open settings panel
        Debug.Log("Settings panel opened.");
        SoundManager.Instance.PlaySound("click");
    }

    public void OnAbout() {
        // Open AI history info
        Debug.Log("About AI opened.");
        SoundManager.Instance.PlaySound("click");
    }
    
    public void OnExit()
    {
        // Open AI history info
        Debug.Log("Quitting Game");
        SoundManager.Instance.PlaySound("click");
    }
}
