using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class TFQuestion
{
    public string id;
    public string prompt;
    public bool correct;
    public string aiLogic;
}

[System.Serializable]
public class TFLevelData
{
    public string title;
    public string format;
    public List<TFQuestion> questions;
}

public class TFLevelManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI aiAnswerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI questionCounter;

    [Header("Buttons")]
    public Button trueButton;
    public Button falseButton;

    [Header("Settings")]
    public float roundTime = 10f;
    public int correctPoints = 10;
    public int wrongPoints = -4;
    [Range(0f,1f)] public float aiCorrectProbability = 0.8f;
    public float autoNextDelay = 2f; // Time before auto-advance

    private List<TFQuestion> questions;
    private int currentIndex = 0;
    private float timeRemaining;
    private bool answered = false;
    private int score = 0;

    void Start()
    {
        LoadQuestions();
        score = 0;
        UpdateScoreText();
        DisplayQuestion();
    }

    void Update()
    {
        if (answered) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerText();

        if (timeRemaining <= 0f)
        {
            CheckAnswer(false);
        }
    }

    void LoadQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Level02_true_false_ai_ml_100.json");
        string json = File.ReadAllText(path);
        TFLevelData data = JsonUtility.FromJson<TFLevelData>(json);

        // Shuffle questions
        questions = data.questions;
        for (int i = 0; i < questions.Count; i++)
        {
            int j = Random.Range(i, questions.Count);
            var tmp = questions[i];
            questions[i] = questions[j];
            questions[j] = tmp;
        }
    }

    void DisplayQuestion()
    {
        var q = questions[currentIndex];

        questionText.text = q.prompt;
        aiAnswerText.text = string.Empty;
        questionCounter.text = $"Q {currentIndex + 1}/{questions.Count}";

        timeRemaining = roundTime;
        answered = false;
        UpdateTimerText();

        trueButton.interactable = true;
        falseButton.interactable = true;

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();
        trueButton.onClick.AddListener(() => CheckAnswer(true));
        falseButton.onClick.AddListener(() => CheckAnswer(false));
    }

    void CheckAnswer(bool selected)
    {
        if (answered) return;
        answered = true;

        var q = questions[currentIndex];
        bool isPlayerCorrect = (selected == q.correct);

        // AI decision
        bool aiSelected = Random.value <= aiCorrectProbability ? q.correct : !q.correct;
        bool isAICorrect = (aiSelected == q.correct);

        // Update score
        score += isPlayerCorrect ? correctPoints : wrongPoints;
        UpdateScoreText();

        // Display results
        string playerText = selected ? "True" : "False";
        string aiText = aiSelected ? "True" : "False";
        aiAnswerText.text =
            $"You: {playerText} ({(isPlayerCorrect ? "Correct" : "Wrong")})\n" +
            $"🧠 AI: {aiText} ({(isAICorrect ? "Correct" : "Wrong")})";

        // Disable inputs
        trueButton.interactable = false;
        falseButton.interactable = false;

        // Auto advance
        StartCoroutine(AutoAdvanceNext());
    }

    System.Collections.IEnumerator AutoAdvanceNext()
    {
        yield return new WaitForSeconds(autoNextDelay);
        MoveToNextQuestion();
    }

    void MoveToNextQuestion()
    {
        currentIndex++;
        if (currentIndex >= questions.Count)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HomePanelScene");
        }
        else
        {
            DisplayQuestion();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}s";
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}
