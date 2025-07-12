using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public GameObject comingSoonPanel;

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
        comingSoonPanel.SetActive(false);

        defaultHumanColor = humanScoreText.color;
        defaultAIColor = aIScoreText.color;

        humanScore = 0;
        aIScore = 0;
        UpdateScoreText();

        StartCoroutine(LoadQuestionsFromStreamingAssets());
    }

    void Update()
    {
        if (answered || questions == null || questions.Count == 0) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerText();

        if (timeRemaining <= 0f)
        {
            CheckAnswer(false); // default answer if time runs out
        }
    }

    IEnumerator LoadQuestionsFromStreamingAssets()
    {
        string fileName = "Level02_true_false_ai_ml_100.json";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string json = "";

#if UNITY_ANDROID
        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load JSON on Android: " + request.error);
                comingSoonPanel.SetActive(true);
                trueFalsePanel.SetActive(false);
                yield break;
            }

            json = request.downloadHandler.text;
        }
#else
        if (File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("File not found at: " + filePath);
            comingSoonPanel.SetActive(true);
            trueFalsePanel.SetActive(false);
            yield break;
        }
#endif

        TFLevelData data = JsonUtility.FromJson<TFLevelData>(json);
        questions = data.questions;

        if (questions == null || questions.Count < 1)
        {
            Debug.LogWarning("No questions found. Showing Coming Soon panel.");
            comingSoonPanel.SetActive(true);
            trueFalsePanel.SetActive(false);
            yield break;
        }

        // Shuffle questions
        for (int i = 0; i < questions.Count; i++)
        {
            int j = Random.Range(i, questions.Count);
            var temp = questions[i];
            questions[i] = questions[j];
            questions[j] = temp;
        }

        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (questions == null || currentIndex >= questions.Count) return;

        TFQuestion q = questions[currentIndex];
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

        SoundManager.Instance?.PlaySound("Click");

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

        Debug.Log($"Player: {(isPlayerCorrect ? "Correct" : "Wrong")} | AI: {(isAICorrect ? "Correct" : "Wrong")}");

        trueButton.interactable = false;
        falseButton.interactable = false;

        StartCoroutine(AutoAdvanceNext());
    }

    IEnumerator BlinkScoreText(TextMeshProUGUI text, Color originalColor)
    {
        text.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);
        text.color = originalColor;
    }

    IEnumerator AutoAdvanceNext()
    {
        yield return new WaitForSeconds(autoNextDelay);
        MoveToNextQuestion();
    }

    void MoveToNextQuestion()
    {
        currentIndex++;
        if (currentIndex >= questions.Count)
        {
            comingSoonPanel.SetActive(true);
        }
        else
        {
            DisplayQuestion();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    void UpdateScoreText()
    {
        if (humanScoreText != null)
            humanScoreText.text = $"You: {humanScore}";

        if (aIScoreText != null)
            aIScoreText.text = $"AI: {aIScore}";
    }
}
