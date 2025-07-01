using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePanelManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject loadingPanel;
    public GameObject levelButtonPrefab;
    public Transform levelGrid;
    public Button continueButton;

    public int totalLevels = 7;

    private void Start()
    {
        homePanel.SetActive(true);
        loadingPanel.SetActive(false);
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject btn = Instantiate(levelButtonPrefab, levelGrid);
            btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"Level {i}";
            int levelIndex = i;

            btn.GetComponent<Button>().onClick.AddListener(() => {
                LoadLevel(levelIndex);
            });

            // TODO: Set lock, completed icon, etc.
        }
    }

    public void LoadLevel(int levelNumber)
    {
        Debug.Log($"Loading Level {levelNumber}");
        // SceneManager.LoadScene($"Level{levelNumber}");
    }

    public void OnContinue()
    {
        int nextUnplayedLevel = GetNextUnplayedLevel();
        LoadLevel(nextUnplayedLevel);
    }

    private int GetNextUnplayedLevel()
    {
        // Replace with real save logic
        return 1; // Default: start from Level 1
    }

    public void OnBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        loadingPanel.SetActive(true);
        homePanel.SetActive(false);
    }
}
