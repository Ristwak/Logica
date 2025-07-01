using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class MCQQuestion
{
    public string id;
    public string prompt;
    public List<string> options;
    public string correct;
    public string aiLogic;
}

[System.Serializable]
public class MCQLevelData
{
    public string title;
    public string format;
    public List<MCQQuestion> questions;
}

public class MCQLevelManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI aiAnswerText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI justificationText;
    public GameObject feedbackPanel;
    public Button nextButton;
    public TextMeshProUGUI questionCounter;

    public GameObject optionButtonPrefab;       // 🔹 Prefab of the button
    public Transform optionsContainer;          // 🔹 Parent with VerticalLayoutGroup

    private List<MCQQuestion> questions;
    private int currentIndex = 0;

    void Start()
    {
        LoadQuestions();
        DisplayQuestion();
        nextButton.onClick.AddListener(OnNextQuestion);
    }

    void LoadQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Level01_MCQ.json");
        string json = File.ReadAllText(path);
        questions = JsonUtility.FromJson<MCQLevelData>(json).questions;
    }

    void DisplayQuestion()
    {
        var q = questions[currentIndex];
        questionText.text = q.prompt;
        aiAnswerText.text = "";
        questionCounter.text = $"Q {currentIndex + 1} / {questions.Count}";
        feedbackPanel.SetActive(false);

        ClearOptions();

        foreach (var option in q.options)
        {
            GameObject buttonGO = Instantiate(optionButtonPrefab, optionsContainer);
            buttonGO.SetActive(true);

            var button = buttonGO.GetComponent<Button>();
            var label = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            label.text = option;

            string chosen = option; // local copy for lambda
            button.onClick.AddListener(() => CheckAnswer(chosen));
        }
    }

    void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void CheckAnswer(string selected)
    {
        var q = questions[currentIndex];
        bool isCorrect = selected == q.correct;

        feedbackText.text = isCorrect ? "✅ Correct!" : "❌ Incorrect.";
        justificationText.text = q.aiLogic;
        aiAnswerText.text = $"🧠 AI says: {q.correct}";
        feedbackPanel.SetActive(true);

        // Optionally disable all buttons after answer
        foreach (Transform child in optionsContainer)
        {
            var btn = child.GetComponent<Button>();
            if (btn != null) btn.interactable = false;
        }
    }

    public void OnNextQuestion()
    {
        currentIndex++;
        if (currentIndex >= questions.Count)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HomeScene");
        }
        else
        {
            DisplayQuestion();
        }
    }
}
