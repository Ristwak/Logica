using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject trueFalsePanel;
    public GameObject exitPanel;
    public GameObject comingSoonPanel;

    private bool isComingSoonActive = false;

    void Start()
    {
        trueFalsePanel.SetActive(true);
        exitPanel.SetActive(false);
    }

    void Update()
    {
        if (comingSoonPanel.activeSelf)
            isComingSoonActive = true;
        if (Input.GetKeyDown(KeyCode.Escape)) // Android back button
        {
            if (!exitPanel.activeSelf)
            {
                exitPanel.SetActive(true);
                comingSoonPanel.SetActive(false);
            }
        }

        if (exitPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    public void OnExitYes()
    {
        SoundManager.Instance.PlaySound("Click");
        Application.Quit();
        Debug.Log("Game closed.");
    }

    public void OnExitNo()
    {
        SoundManager.Instance?.PlaySound("Click");
        if(isComingSoonActive)
        {
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            exitPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}
