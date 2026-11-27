# Logica

**Logica** is an educational mobile game developed for Homi Lab. It explores the evolution of Artificial Intelligence through logic-based challenges and comparisons between rule-based AI and learning-based AI.

Players compete against an AI opponent in True/False challenges, testing their understanding of AI and Machine Learning concepts.

## 🎮 Game Overview

Logica introduces two different approaches to Artificial Intelligence:

- **RuleBot:** Represents classical AI, based on predefined rules, symbolic reasoning, and logical decisions.
- **LearnBot:** Represents modern learning-based AI, focusing on patterns, probabilities, and adaptability.

The game places players in a virtual learning environment where they answer AI and ML-related questions and compare their performance against an AI opponent.

## ✨ Features

- True/False question-based gameplay.
- AI opponent with configurable answer accuracy.
- Randomized question order.
- Countdown timer for each question.
- Player and AI scoring system.
- Visual score feedback.
- Automatic progression between questions.
- Audio feedback and background music.
- Menu, About, and Coming Soon panels.
- Android-compatible StreamingAssets question loading.

## 🛠️ Built With

- **Game Engine:** Unity 2022.3.39f1
- **Programming Language:** C#
- **UI:** Unity UI and TextMesh Pro
- **Data Storage:** JSON
- **Platform:** Android

## 📂 Project Structure

```text
Logica/
├── Assets/
│   ├── Audio/
│   ├── Scenes/
│   │   ├── MenuScene.unity
│   │   └── GameScene.unity
│   ├── Script/
│   │   ├── AboutGameManager.cs
│   │   ├── GameManager.cs
│   │   ├── LoadingPanelManager.cs
│   │   ├── ScreenOrientationSetter.cs
│   │   ├── SoundManager.cs
│   │   ├── GameScripts/
│   │   │   └── TFLevelManager.cs
│   │   └── MainMenu/
│   │       └── MainMenuManager.cs
│   ├── StreamingAssets/
│   │   └── Level02_true_false_ai_ml_100.json
│   └── UI/
├── Packages/
└── ProjectSettings/
```

## 🕹️ Gameplay

1. Launch the game through the Menu Scene.
2. Start the True/False challenge.
3. Read the displayed AI or Machine Learning statement.
4. Select True or False before the timer expires.
5. Earn points for correct answers.
6. The AI opponent independently makes its prediction.
7. Compare the player's score with the AI score.
8. Continue until all available questions have been completed.

### Scoring System

| Setting | Default Value |
|---|---:|
| Time per question | 10 seconds |
| Points per correct answer | 10 |
| AI correct probability | 80% |
| Delay before next question | 2 seconds |

These values are configurable through the `TFLevelManager` Inspector.

## 📄 Question Dataset

Questions are stored in JSON format under `Assets/StreamingAssets/`.

Current dataset:

- File: `Level02_true_false_ai_ml_100.json`
- Available questions: **10**
- Format: True/False
- Question order: Randomized when a session starts.

### JSON Structure

```json
{
  "levelId": 2,
  "title": "True or False: AI & ML Basics",
  "format": "true_false",
  "questions": [
    {
      "id": "Q1",
      "prompt": "Example question statement",
      "correct": true
    }
  ]
}
```

The current dataset contains 10 question records. The `100` in the filename does not represent the current number of available questions.

## 🧩 Main Scripts

| Script | Responsibility |
|---|---|
| `TFLevelManager.cs` | Question loading, timer, answer evaluation, AI decisions, scoring and progression |
| `GameManager.cs` | Gameplay UI panels and exit handling |
| `MainMenuManager.cs` | Main menu navigation |
| `SoundManager.cs` | Audio playback, music and volume management |
| `AboutGameManager.cs` | About section content |
| `LoadingPanelManager.cs` | Loading panel and loading animation |
| `ScreenOrientationSetter.cs` | Landscape orientation configuration |

## ⚙️ Setup Instructions

1. Clone or download the repository.
2. Open the project using Unity **2022.3.39f1**.
3. Allow Unity to import the project assets and packages.
4. Open `Assets/Scenes/MenuScene.unity`.
5. Ensure the required Inspector references, audio sources, UI elements and question dataset are assigned.
6. Check that both `MenuScene` and `GameScene` are enabled in Build Settings.
7. Build and run the project on the target platform.

## 📱 Platform Compatibility

The project includes Android-specific JSON loading through `UnityWebRequest` and file-based loading for non-Android platforms.

The game is configured for landscape orientation.

## 📌 Project Information

- **Project:** Logica
- **Developer:** Ristwak Pandey
- **Organization:** Homi Lab
- **Category:** Educational / AI & Machine Learning
- **Engine:** Unity
- **Status:** Existing project with a True/False gameplay implementation

---

*Logica explores how machines reason, learn, and make decisions through interactive AI and Machine Learning challenges.*
