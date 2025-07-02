using System.Collections;
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
    public string correct;          // aiLogic removed
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
    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI aiAnswerText;
    public TextMeshProUGUI feedbackText;
    public Button nextButton;
    public TextMeshProUGUI questionCounter;

    [Header("Options Container")]
    public GameObject optionButtonPrefab;
    public Transform optionsContainer;

    [Header("AI Settings")]
    public float aiMinThinkTime = 1.0f;
    public float aiMaxThinkTime = 3.0f;
    [Range(0f,1f)]
    public float aiCorrectProbability = 0.8f;

    private List<MCQQuestion> questions;
    private int currentIndex = 0;
    private string playerChoice;

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
        questionText.text       = q.prompt;
        aiAnswerText.text       = "";
        feedbackText.text       = "";            // Clear previous feedback
        questionCounter.text    = $"Q {currentIndex + 1} / {questions.Count}";

        ClearOptions();
        nextButton.interactable = false;          // Disable Next until feedback shown

        foreach (var option in q.options)
        {
            var btnGO = Instantiate(optionButtonPrefab, optionsContainer);
            var label = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            var button = btnGO.GetComponent<Button>();

            label.text = option;
            button.interactable = true;
            string chosen = option;
            button.onClick.AddListener(() => OnOptionSelected(chosen));
        }
    }

    void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
            Destroy(child.gameObject);
    }

    void OnOptionSelected(string chosen)
    {
        playerChoice = chosen;
        // Disable all option buttons
        foreach (Transform child in optionsContainer)
        {
            var btn = child.GetComponent<Button>();
            if (btn != null) btn.interactable = false;
        }

        // Show AI is thinking
        aiAnswerText.text = "🧠 AI is thinking...";

        // Start AI response
        StartCoroutine(AIResponseCoroutine());
    }

    IEnumerator AIResponseCoroutine()
    {
        // Random thinking time
        float thinkTime = Random.Range(aiMinThinkTime, aiMaxThinkTime);
        yield return new WaitForSeconds(thinkTime);

        var q = questions[currentIndex];
        string aiChoice;

        // Determine AI answer with given probability
        if (Random.value <= aiCorrectProbability)
        {
            aiChoice = q.correct;
        }
        else
        {
            // pick a random wrong option
            List<string> wrongs = new List<string>(q.options);
            wrongs.Remove(q.correct);
            aiChoice = wrongs[Random.Range(0, wrongs.Count)];
        }

        // Display AI answer
        aiAnswerText.text = $"🧠 AI says: {aiChoice}";

        // Provide combined feedback
        bool playerCorrect = (playerChoice == q.correct);
        bool aiCorrect     = (aiChoice    == q.correct);

        feedbackText.text =
            $"{(playerCorrect ? "✅ You: Correct" : "❌ You: Wrong")}\n" +
            $"{(aiCorrect     ? "🤖✅ AI: Correct" : "🤖❌ AI: Wrong")}";

        // Enable Next button after feedback
        nextButton.interactable = true;
    }

    public void OnNextQuestion()
    {
        currentIndex++;
        if (currentIndex >= questions.Count)
            UnityEngine.SceneManagement.SceneManager.LoadScene("HomeScene");
        else
            DisplayQuestion();
    }
}