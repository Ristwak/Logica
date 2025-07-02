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
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI humanScoreText;
    public TextMeshProUGUI aIScoreText;
    public GameObject trueFalsePanel;
    public GameObject exitPanel;

    [Header("Buttons")]
    public Button trueButton;
    public Button falseButton;

    [Header("Settings")]
    public float roundTime = 10f;
    public int correctPoints = 10;
    [Range(0f, 1f)] public float aiCorrectProbability = 0.8f;
    public float autoNextDelay = 2f;

    [Header("Visual Feedback")]
    public Color blinkColor = Color.red;
    public float blinkDuration = 0.4f;

    private Color defaultHumanColor;
    private Color defaultAIColor;

    private List<TFQuestion> questions;
    private int currentIndex = 0;
    private float timeRemaining;
    private bool answered = false;
    private int humanScore = 0;
    private int aIScore = 0;

    void Start()
    {
        trueFalsePanel.SetActive(true);
        exitPanel.SetActive(false);
        LoadQuestions();
        humanScore = 0;
        aIScore = 0;

        defaultHumanColor = humanScoreText.color;
        defaultAIColor = aIScoreText.color;

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
        bool aiSelected = Random.value <= aiCorrectProbability ? q.correct : !q.correct;
        bool isAICorrect = (aiSelected == q.correct);

        if (isPlayerCorrect)
        {
            humanScore += correctPoints;
            StartCoroutine(BlinkScoreText(humanScoreText, defaultHumanColor));
        }

        if (isAICorrect)
        {
            aIScore += correctPoints;
            StartCoroutine(BlinkScoreText(aIScoreText, defaultAIColor));
        }

        UpdateScoreText();

        Debug.Log($"Player answered: {(isPlayerCorrect ? "Correct" : "Wrong")} | AI answered: {(isAICorrect ? "Correct" : "Wrong")}");

        trueButton.interactable = false;
        falseButton.interactable = false;

        StartCoroutine(AutoAdvanceNext());
    }

    System.Collections.IEnumerator BlinkScoreText(TextMeshProUGUI text, Color originalColor)
    {
        text.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);
        text.color = originalColor;
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
        if (humanScoreText != null)
            humanScoreText.text = $"You: {humanScore}";

        if (aIScoreText != null)
            aIScoreText.text = $"AI: {aIScore}";
    }

}
