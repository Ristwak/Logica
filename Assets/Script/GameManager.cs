using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject trueFalsePanel;
    public GameObject exitPanel;
    public GameObject comingSoonPanel;

    private bool isComingSoonActive;

    private void Start()
    {
        if (trueFalsePanel != null)
            trueFalsePanel.SetActive(true);

        if (exitPanel != null)
            exitPanel.SetActive(false);
    }

    private void Update()
    {
        if (comingSoonPanel != null &&
            comingSoonPanel.activeSelf)
        {
            isComingSoonActive = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitPanel != null && !exitPanel.activeSelf)
            {
                exitPanel.SetActive(true);

                if (comingSoonPanel != null)
                    comingSoonPanel.SetActive(false);
            }
        }

        if (exitPanel != null && exitPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    public void OnExitYes()
    {
        SoundManager.Instance?.PlaySound("Click");

        Application.Quit();

        Debug.Log("Game closed.");
    }

    public void OnExitNo()
    {
        SoundManager.Instance?.PlaySound("Click");

        Time.timeScale = 1f;

        if (isComingSoonActive)
        {
            SceneManager.LoadScene("MenuScene");
            return;
        }

        if (exitPanel != null)
            exitPanel.SetActive(false);
    }
}