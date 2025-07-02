using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject aboutPanel;
    public GameObject loadingPanel;

    void Start()
    {
        menuPanel.SetActive(true);
        loadingPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    public void OnStartGame()
    {
        // Load first level or scene
        Debug.Log("Starting Game");
        menuPanel.SetActive(false);
        SceneManager.LoadScene("GameScene");
        loadingPanel.SetActive(true);
        menuPanel.SetActive(false);
        SoundManager.Instance.PlaySound("click");
    }

    public void OnLearn() {
        // Open tutorial panel
        menuPanel.SetActive(false);
        aboutPanel.SetActive(true);
        SoundManager.Instance.PlaySound("click");
    }
    
    public void OnBack()
    {
        // Return to main menu
        aboutPanel.SetActive(false);
        menuPanel.SetActive(true);
        SoundManager.Instance.PlaySound("click");
    }

    public void OnAbout()
    {
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
