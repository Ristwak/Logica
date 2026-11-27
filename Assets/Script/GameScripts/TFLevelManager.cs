using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class TFQuestion
{
    public string id;
    public string prompt;
    public bool correct;
    public string aiLogic;
}

[Serializable]
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
    private int currentIndex;
    private float timeRemaining;
    private bool answered;
    private int humanScore;
    private int aIScore;

    private Coroutine autoAdvanceCoroutine;
    private Coroutine humanBlinkCoroutine;
    private Coroutine aiBlinkCoroutine;

    private const string QuestionFileName =
        "Level02_true_false_ai_ml_100.json";

    private void Start()
    {
        if (!ValidateReferences())
            return;

        trueFalsePanel.SetActive(true);
        exitPanel.SetActive(false);
        comingSoonPanel.SetActive(false);

        defaultHumanColor = humanScoreText.color;
        defaultAIColor = aIScoreText.color;

        humanScore = 0;
        aIScore = 0;
        currentIndex = 0;
        answered = true;

        UpdateScoreText();

        StartCoroutine(LoadQuestionsFromStreamingAssets());
    }

    private void Update()
    {
        if (answered || questions == null ||
            currentIndex >= questions.Count)
            return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerText();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();
            CheckAnswer(false);
        }
    }

    private bool ValidateReferences()
    {
        bool valid = true;

        if (questionText == null ||
            timerText == null ||
            humanScoreText == null ||
            aIScoreText == null ||
            trueFalsePanel == null ||
            exitPanel == null ||
            comingSoonPanel == null ||
            trueButton == null ||
            falseButton == null)
        {
            Debug.LogError(
                "TFLevelManager: One or more Inspector references are missing."
            );

            valid = false;
        }

        if (roundTime <= 0f)
        {
            Debug.LogError(
                "TFLevelManager: Round time must be greater than zero."
            );

            valid = false;
        }

        return valid;
    }

    private IEnumerator LoadQuestionsFromStreamingAssets()
    {
        string filePath = Path.Combine(
            Application.streamingAssetsPath,
            QuestionFileName
        );

        string json = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "Failed to load JSON: " + request.error
                );

                ShowComingSoonPanel();
                yield break;
            }

            json = request.downloadHandler.text;
        }
#else
        if (!File.Exists(filePath))
        {
            Debug.LogError(
                "Question file not found: " + filePath
            );

            ShowComingSoonPanel();
            yield break;
        }

        try
        {
            json = File.ReadAllText(filePath);
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "Failed to read question file: " + exception.Message
            );

            ShowComingSoonPanel();
            yield break;
        }
#endif

        TFLevelData data = null;

        try
        {
            data = JsonUtility.FromJson<TFLevelData>(json);
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "Question JSON parsing failed: " + exception.Message
            );

            ShowComingSoonPanel();
            yield break;
        }

        if (data == null ||
            data.questions == null ||
            data.questions.Count == 0)
        {
            Debug.LogWarning(
                "No valid questions found in the JSON file."
            );

            ShowComingSoonPanel();
            yield break;
        }

        questions = data.questions;

        ShuffleQuestions();
        DisplayQuestion();
    }

    private void ShuffleQuestions()
    {
        for (int i = questions.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            TFQuestion temp = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }

    private void DisplayQuestion()
    {
        if (questions == null ||
            currentIndex < 0 ||
            currentIndex >= questions.Count)
        {
            ShowComingSoonPanel();
            return;
        }

        TFQuestion question = questions[currentIndex];

        questionText.text = question.prompt;

        timeRemaining = roundTime;
        answered = false;

        UpdateTimerText();

        trueButton.interactable = true;
        falseButton.interactable = true;

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        trueButton.onClick.AddListener(
            () => CheckAnswer(true)
        );

        falseButton.onClick.AddListener(
            () => CheckAnswer(false)
        );
    }

    private void CheckAnswer(bool selected)
    {
        if (answered ||
            questions == null ||
            currentIndex >= questions.Count)
            return;

        answered = true;

        SoundManager.Instance?.PlaySound("Click");

        TFQuestion question = questions[currentIndex];

        bool isPlayerCorrect = selected == question.correct;

        bool aiSelected =
            UnityEngine.Random.value <= aiCorrectProbability
                ? question.correct
                : !question.correct;

        bool isAICorrect = aiSelected == question.correct;

        if (isPlayerCorrect)
        {
            humanScore += correctPoints;

            if (humanBlinkCoroutine != null)
                StopCoroutine(humanBlinkCoroutine);

            humanBlinkCoroutine = StartCoroutine(
                BlinkScoreText(humanScoreText, defaultHumanColor)
            );
        }

        if (isAICorrect)
        {
            aIScore += correctPoints;

            if (aiBlinkCoroutine != null)
                StopCoroutine(aiBlinkCoroutine);

            aiBlinkCoroutine = StartCoroutine(
                BlinkScoreText(aIScoreText, defaultAIColor)
            );
        }

        UpdateScoreText();

        Debug.Log(
            $"Player: {(isPlayerCorrect ? "Correct" : "Wrong")} | " +
            $"AI: {(isAICorrect ? "Correct" : "Wrong")}"
        );

        trueButton.interactable = false;
        falseButton.interactable = false;

        if (autoAdvanceCoroutine != null)
            StopCoroutine(autoAdvanceCoroutine);

        autoAdvanceCoroutine = StartCoroutine(AutoAdvanceNext());
    }

    private IEnumerator BlinkScoreText(
        TextMeshProUGUI text,
        Color originalColor)
    {
        if (text == null)
            yield break;

        text.color = blinkColor;

        yield return new WaitForSeconds(blinkDuration);

        text.color = originalColor;
    }

    private IEnumerator AutoAdvanceNext()
    {
        yield return new WaitForSeconds(autoNextDelay);

        MoveToNextQuestion();
    }

    private void MoveToNextQuestion()
    {
        currentIndex++;

        if (questions == null || currentIndex >= questions.Count)
        {
            ShowComingSoonPanel();
            return;
        }

        DisplayQuestion();
    }

    private void ShowComingSoonPanel()
    {
        if (comingSoonPanel != null)
            comingSoonPanel.SetActive(true);

        if (trueFalsePanel != null)
            trueFalsePanel.SetActive(false);
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text =
                Mathf.CeilToInt(timeRemaining).ToString();
        }
    }

    private void UpdateScoreText()
    {
        if (humanScoreText != null)
            humanScoreText.text = $"You: {humanScore}";

        if (aIScoreText != null)
            aIScoreText.text = $"AI: {aIScore}";
    }

    private void OnDisable()
    {
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }

        if (humanBlinkCoroutine != null)
        {
            StopCoroutine(humanBlinkCoroutine);
            humanBlinkCoroutine = null;
        }

        if (aiBlinkCoroutine != null)
        {
            StopCoroutine(aiBlinkCoroutine);
            aiBlinkCoroutine = null;
        }
    }
}