using UnityEngine;
using TMPro;

/// <summary>
/// Populates the About panel with game information.
/// </summary>
public class AboutGameManager : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the TMP_Text component for the About section.")]
    public TMP_Text aboutText;

    void Start()
    {
        if (aboutText == null)
        {
            Debug.LogError("AboutGameManager: 'aboutText' reference is missing.");
            return;
        }

        aboutText.text =
            "<b>ABOUT LOGICA</b>\n" +
            "Logica is a fast-paced True/False logic quiz where you compete against an adaptive AI opponent.\n\n" +

            "<b>HOW TO PLAY</b>\n" +
            "• A statement will appear on screen.\n" +
            "• Tap 'True' or 'False' within the time limit.\n" +
            "• The AI will think and then reveal its answer.\n\n" +

            "<b>SCORE</b>\n" +
            "• Correct: +10 points\n" +
            "• Incorrect: -4 points\n" +
            "• No answer (timeout): -4 points\n\n" +

            "<b>FEATURES</b>\n" +
            "• AI competitor that learns over time.\n" +
            "• 10-second countdown for each question.\n" +
            "• Randomized question order for replayability.\n\n" +

            "Challenge your logic skills, outsmart the AI, and see how high you can score!";
    }
}
