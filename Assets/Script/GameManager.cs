using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject trueFalsePanel;
    public GameObject exitPanel;

    void Start()
    {
        trueFalsePanel.SetActive(true);
        exitPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Android back button
        {
            if (!exitPanel.activeSelf)
                exitPanel.SetActive(true);
        }

        if (exitPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    public void OnExitYes()
    {
        Application.Quit();
        Debug.Log("Game closed.");
    }

    public void OnExitNo()
    {
        exitPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game if exit panel is closed
    }
}
