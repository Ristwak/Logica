using UnityEngine;
using TMPro;

/// <summary>
/// Populates the About panel with detailed game info.
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
            "<b>About Logica</b>\n" +
            "Logica is a futuristic logic battle game where two AI bots face off — one powered by hardcoded rules (symbolic logic), the other by learning algorithms (machine learning).\n" +
            "As a human player, you jump into this arena to challenge both bots, testing your reasoning skills while witnessing how AI thinks, learns, and sometimes even makes mistakes.\n\n" +

            "<b>What’s the Game About?</b>\n" +
            "Set inside a virtual lab, Logica explores the oldest question in Artificial Intelligence:\n\n" +
            "<i>Can machines think?</i>\n\n" +
            "You take on logic challenges that simulate how early AI (like the Logic Theorist) solved problems using fixed rules, while also observing how modern learning-based AI approaches the same problems differently.\n\n" +

            "<b>Two Bots, Two Minds</b>\n" +
            "<b>RuleBot</b> – Thinks like classic AI: deductive, symbolic, precise.\n" +
            "<b>LearnBot</b> – Thinks like a modern ML system: pattern-based, probabilistic, adaptive.\n\n" +
            "Sometimes they agree.\n" +
            "Sometimes they contradict each other.\n" +
            "Sometimes… you’ll be smarter than both.\n\n" +

            "<b>What You'll Learn</b>\n" +
            "• How symbolic logic forms the backbone of early AI\n" +
            "• Where rule-based AI excels — and fails\n" +
            "• How machine learning approaches uncertainty\n" +
            "• Why understanding both is key to building modern intelligent systems\n\n" +

            "<b>Built for PG Students in AI/ML</b>\n" +
            "Logica isn’t just a game. It’s a lab disguised as fun.\n" +
            "Designed for postgraduate students, it strengthens your grasp of:\n" +
            "• Deductive logic\n" +
            "• Symbolic reasoning\n" +
            "• Inference patterns\n" +
            "• AI vs ML paradigms\n\n" +

            "<b>Ready to Play?</b>\n" +
            "Step into the arena. Outsmart the bots.\n" +
            "Learn how AI learns — and how it used to think.\n" +
            "Welcome to Logica.";
    }
}
