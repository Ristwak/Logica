using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject aboutPanel;
    public GameObject loadingPanel;

    private void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(true);

        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        if (aboutPanel != null)
            aboutPanel.SetActive(false);
    }

    public void OnStartGame()
    {
        Debug.Log("Starting Game");

        SoundManager.Instance?.PlaySound("click");

        if (menuPanel != null)
            menuPanel.SetActive(false);

        SceneManager.LoadScene("GameScene");
    }

    public void OnLearn()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (aboutPanel != null)
            aboutPanel.SetActive(true);

        SoundManager.Instance?.PlaySound("click");
    }

    public void OnBack()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(false);

        if (menuPanel != null)
            menuPanel.SetActive(true);

        SoundManager.Instance?.PlaySound("click");
    }

    public void OnAbout()
    {
        Debug.Log("About AI opened.");

        SoundManager.Instance?.PlaySound("click");
    }

    public void OnExit()
    {
        Debug.Log("Quitting Game");

        SoundManager.Instance?.PlaySound("click");

        Application.Quit();
    }
}