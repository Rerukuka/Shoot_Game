# AnimalsGame — Unity Quiz Game

A three-level educational quiz game built with **Unity 6 and C#**.

The game focuses on animal-related questions and combines three different quiz mechanics:

1. **Multiple Choice**
2. **True / False**
3. **Word Game**

The player's score and correct-answer streak are preserved across all three levels using a persistent `GameSession` object.

---

## Overview

The complete gameplay flow is:

```text
Main Menu
    │
    │ Start
    ▼
Level 1
Multiple Choice
10 Questions
    │
    ▼
Level 2
True / False
10 Questions
    │
    ▼
Level 3
Word Game
10 Questions
    │
    ▼
Final Score
    │
    │ 5 seconds
    ▼
Main Menu
```

The project contains **30 questions in total**.

---

# Features

The current implementation includes:

* Main menu
* Start button
* Quit button
* Three sequential quiz levels
* Multiple-choice questions
* True/False questions
* Word-answer input
* JSON-based question data
* Persistent score between scenes
* Correct-answer streak system
* Bonus scoring for consecutive correct answers
* Correct / wrong feedback
* Green and red feedback text
* Correct-answer sound effect
* Wrong-answer sound effect
* Final score screen
* Automatic level progression
* TextMesh Pro UI
* Unity UI buttons and input fields
* URP 2D project configuration

---

# Technology Stack

```text
Engine:          Unity 6
Unity Version:  6000.0.43f1
Language:        C#
Rendering:       Universal Render Pipeline 17.0.4
UI:              Unity UI + TextMesh Pro
Input:           Unity Input System 1.13.1
Data Format:     JSON
Audio:           MP3 / Unity AudioSource
```

---

# Unity Version

The project was created with:

```text
Unity 6000.0.43f1
```

Revision:

```text
6000.0.43f1 (97272b72f107)
```

Using the same Unity version is recommended to avoid unnecessary project or package upgrades.

---

# Project Structure

```text
QuizGame_5-main/
│
├── Assets/
│   │
│   ├── Scripts/
│   │   ├── GameSession.cs
│   │   │
│   │   ├── main/
│   │   │   └── GameManager.cs
│   │   │
│   │   ├── 1/
│   │   │   ├── Question1.cs
│   │   │   ├── QuestionUI1.cs
│   │   │   └── GameManager1.cs
│   │   │
│   │   ├── 2/
│   │   │   ├── Question2.cs
│   │   │   └── QuestionUI2.cs
│   │   │
│   │   └── 3/
│   │       ├── Question3.cs
│   │       └── QuestionUI3.cs
│   │
│   ├── Resources/
│   │   ├── level1_questions.json
│   │   ├── level2_questions.json
│   │   └── level3_questions.json
│   │
│   ├── Audio/
│   │   ├── correct.mp3
│   │   └── wrong.mp3
│   │
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Level1_MultipleChoice.unity
│   │   ├── Level2_TrueFalse.unity
│   │   └── Level3_WordGame.unity
│   │
│   ├── Settings/
│   ├── TextMesh Pro/
│   └── InputSystem_Actions.inputactions
│
├── Packages/
├── ProjectSettings/
├── UserSettings/
├── Logs/
├── QuizGame.sln
├── Assembly-CSharp.csproj
└── UpgradeLog.htm
```

---

# Game Scenes

The project contains four active gameplay scenes.

## Main Menu

```text
Assets/Scenes/MainMenu.unity
```

The menu displays:

```text
Welcome to AnimalsGame
```

and contains:

```text
Start
Quit
```

buttons.

The Start button loads:

```text
Level1_MultipleChoice
```

through build index `1`.

The Quit button calls:

```csharp
Application.Quit();
```

---

## Level 1 — Multiple Choice

```text
Assets/Scenes/Level1_MultipleChoice.unity
```

The first level contains:

```text
10 multiple-choice questions
```

Each question has:

```text
4 answer options
```

Example:

```text
Which animal is the largest land mammal?

A. Elephant
B. Lion
C. Giraffe
D. Tiger
```

The correct answer is stored in JSON as:

```json
{
  "questionText": "Which animal is the largest land mammal?",
  "options": [
    "Elephant",
    "Lion",
    "Giraffe",
    "Tiger"
  ],
  "correctAnswerIndex": 0
}
```

---

# Level 1 Data Model

The C# representation is:

```csharp
[System.Serializable]
public class Question1
{
    public string questionText;
    public string[] options;
    public int correctAnswerIndex;
}
```

Questions are loaded with:

```csharp
Resources.Load<TextAsset>("level1_questions");
```

and parsed using:

```csharp
JsonUtility.FromJson<QuestionList1>()
```

---

# Level 1 Progression

After all 10 questions are completed:

```csharp
SceneManager.LoadScene(
    "Level2_TrueFalse"
);
```

is called automatically.

No Next Level button is required.

---

# Level 2 — True / False

Scene:

```text
Assets/Scenes/Level2_TrueFalse.unity
```

The second level contains:

```text
10 True / False questions
```

Examples include:

```text
Elephants can jump.
→ False

A group of lions is called a pride.
→ True

Sharks are mammals.
→ False

Owls can rotate their heads up to 270 degrees.
→ True
```

---

# Level 2 Data Model

```csharp
[System.Serializable]
public class Question2
{
    public string questionText;
    public bool isTrue;
}
```

Example JSON:

```json
{
  "questionText": "A group of lions is called a pride.",
  "isTrue": true
}
```

The user answers using:

```text
TRUE
FALSE
```

buttons.

---

# Level 2 Progression

After all questions are answered:

```csharp
SceneManager.LoadScene(
    "Level3_WordGame"
);
```

loads the final level automatically.

---

# Level 3 — Word Game

Scene:

```text
Assets/Scenes/Level3_WordGame.unity
```

The player sees a word and must enter the expected animal-related answer.

The interface contains:

```text
Scrambled Word
Input Field
Submit Button
Feedback Text
Score
Final Score
```

---

# Level 3 Data Model

```csharp
[System.Serializable]
public class Question3
{
    public string scrambledWord;
    public string correctWord;
}
```

Example:

```json
{
  "scrambledWord": "RABTIT",
  "correctWord": "RABBIT"
}
```

The user's answer is processed using:

```csharp
inputField.text
    .Trim()
    .ToUpper();
```

The correct answer is also converted to uppercase.

Therefore answer comparison is effectively:

```text
case-insensitive
```

after whitespace is removed from the beginning and end.

---

# Final Score

When Level 3 is finished, the normal gameplay controls are hidden.

The application displays:

```text
Final Score: ...
```

for:

```text
5 seconds
```

and then automatically returns to:

```text
MainMenu
```

using:

```csharp
SceneManager.LoadScene("MainMenu");
```

---

# Question Count

The current repository contains:

| Level     | Type            | Questions |
| --------- | --------------- | --------: |
| Level 1   | Multiple Choice |        10 |
| Level 2   | True / False    |        10 |
| Level 3   | Word Game       |        10 |
| **Total** |                 |    **30** |

---

# Persistent Game Session

The global game state is controlled by:

```text
Assets/Scripts/GameSession.cs
```

The class uses a singleton:

```csharp
public static GameSession Instance;
```

and calls:

```csharp
DontDestroyOnLoad(gameObject);
```

Therefore the same object survives when Unity changes between quiz scenes.

---

# Session Data

The session stores:

```csharp
public int Score = 0;
public int CorrectStreak = 0;
```

This allows:

```text
Level 1 Score
      │
      ▼
Level 2 Score
      │
      ▼
Level 3 Score
      │
      ▼
Final Score
```

instead of resetting the score after every scene.

---

# Scoring System

A normal correct answer gives at least:

```text
10 points
```

The game also provides an increasing bonus for consecutive correct answers.

The code is:

```csharp
GameSession.Instance.CorrectStreak++;

GameSession.Instance.Score +=
    10 +
    (
        GameSession.Instance.CorrectStreak - 1
    );
```

---

# Streak Example

Consecutive correct answers produce:

| Correct Streak | Points |
| -------------: | -----: |
|              1 |     10 |
|              2 |     11 |
|              3 |     12 |
|              4 |     13 |
|              5 |     14 |
|              6 |     15 |

Therefore:

```text
Correct
   │
   ▼
Streak +1
   │
   ▼
Increasing bonus
```

---

# Wrong Answer

When an answer is wrong:

```csharp
GameSession.Instance.CorrectStreak = 0;
```

The player's existing score is not reduced.

Example:

```text
Correct → +10
Correct → +11
Correct → +12

Wrong
  │
  ▼
Streak reset to 0

Next Correct → +10
```

---

# Maximum Score

If all 30 questions are answered correctly without breaking the streak:

```text
Question 1  = 10
Question 2  = 11
Question 3  = 12
...
Question 30 = 39
```

The theoretical maximum score is:

```text
735 points
```

because the streak continues across all three levels.

---

# Feedback System

After an answer is selected, gameplay input is temporarily locked using:

```csharp
isAnswering = true;
```

This prevents multiple answers from being submitted before the next question appears.

---

# Correct Feedback

For a correct answer:

```text
Correct!
```

is displayed in:

```text
Green
```

and the game plays:

```text
Assets/Audio/correct.mp3
```

---

# Wrong Feedback

For a wrong answer:

```text
Wrong!
```

is displayed in:

```text
Red
```

and the game plays:

```text
Assets/Audio/wrong.mp3
```

---

# Feedback Timing

Feedback remains visible for:

```text
1.5 seconds
```

using:

```csharp
yield return new WaitForSeconds(1.5f);
```

The next question is displayed automatically afterward.

---

# Audio System

`GameSession` contains:

```csharp
public AudioClip correctSound;
public AudioClip wrongSound;
```

and an:

```text
AudioSource
```

component.

Sound effects are played using:

```csharp
audioSource.PlayOneShot(correctSound);
```

and:

```csharp
audioSource.PlayOneShot(wrongSound);
```

The Main Menu `Session` object has both included MP3 files assigned.

---

# Complete Gameplay Architecture

```text
                      Main Menu
                          │
                          ▼
                     GameSession
                          │
            Score / Streak / Audio
                          │
                          ▼
               Level 1 Multiple Choice
                          │
                     10 Questions
                          │
                          ▼
                Level 2 True / False
                          │
                     10 Questions
                          │
                          ▼
                   Level 3 Words
                          │
                     10 Questions
                          │
                          ▼
                    Final Score
                          │
                          ▼
                      Main Menu
```

---

# Question Storage

Questions are stored outside the C# source code in:

```text
Assets/Resources/
```

Files:

```text
level1_questions.json
level2_questions.json
level3_questions.json
```

This makes question content easier to edit without changing the gameplay classes.

---

# JSON Loading Flow

```text
JSON File
    │
    ▼
Resources.Load<TextAsset>()
    │
    ▼
JsonUtility.FromJson()
    │
    ▼
Question[]
    │
    ▼
Question UI
```

---

# Build Settings

The enabled scenes are:

```text
0 — MainMenu
1 — Level1_MultipleChoice
2 — Level3_WordGame
3 — Level2_TrueFalse
```

The code primarily changes levels using scene names, so the Level 2 / Level 3 ordering in Build Settings does not affect their normal scripted progression.

The Start button specifically loads build index:

```text
1
```

which correctly corresponds to:

```text
Level1_MultipleChoice
```

---

# Important Score Reset Behavior

`GameSession` uses:

```csharp
DontDestroyOnLoad(gameObject);
```

but the code does not currently contain a dedicated:

```text
ResetGame()
```

or:

```text
ResetScore()
```

method.

After Level 3 finishes, the application returns to `MainMenu`, but the original persistent `GameSession` can still exist.

Therefore starting another game during the same runtime may continue using the previous:

```text
Score
CorrectStreak
```

unless the session object has otherwise been recreated.

A future version should reset the session when starting a new game.

For example:

```csharp
GameSession.Instance.Score = 0;
GameSession.Instance.CorrectStreak = 0;
```

before loading Level 1.

---

# Current Word Game Data Issue

Several entries in:

```text
level3_questions.json
```

are intended to be scrambled words, but some are not mathematically valid anagrams of their answers.

Examples include:

```text
GRETIT     → TIGER
PAEDN      → PANDA
NLEOHPATE  → ELEPHANT
AORGMNEA   → MANGROVE
RABTIT     → RABBIT
BODLHPNI   → DOLPHIN
AKRONGOO   → KANGAROO
```

Some contain:

```text
extra letters
missing letters
incorrect letter counts
```

Also:

```text
GIRAFFE → GIRAFFE
LION    → LION
```

are not scrambled.

The game still works because it only compares the player's typed answer to:

```text
correctWord
```

but the displayed clue data should be corrected for a proper word-scramble game.

---

# Video Background Status

The scenes contain objects named:

```text
Video object
VideoBackground
```

using Unity:

```text
VideoPlayer
```

components.

The scenes reference VideoClip asset GUIDs.

However, the corresponding video files / matching `.meta` assets are **not present in the uploaded repository**.

The project currently contains an:

```text
Assets/Video/
```

directory, but no actual video file is included there.

Therefore the scene VideoPlayer references may appear as missing assets after the project is opened.

The video files should be restored to the project if animated backgrounds are intended to work.

---

# Main Menu

The menu contains:

```text
Welcome to AnimalsGame
```

with:

```text
Start
Quit
```

controls.

The associated script is:

```text
Assets/Scripts/main/GameManager.cs
```

---

# `GameManager.cs`

The class provides:

```csharp
LoadLevelByName(string sceneName)
LoadLevel(int levelIndex)
QuitGame()
```

The Main Menu currently uses:

```text
LoadLevel(1)
```

for Start and:

```text
QuitGame()
```

for Quit.

---

# `GameManager1.cs`

The repository also contains:

```text
Assets/Scripts/1/GameManager1.cs
```

and this component is present in the gameplay scenes.

It contains its own:

```text
Score
CorrectStreak
AudioClips
LoadLevel()
QuitGame()
```

logic.

However, the question scripts actually use:

```text
GameSession.Instance
```

for score, streak, and sound effects.

Therefore `GameManager1` duplicates some functionality and can potentially be removed or refactored to avoid having two different systems responsible for similar game state.

---

# Unity Packages

Important installed packages include:

| Package                   | Version |
| ------------------------- | ------: |
| Universal Render Pipeline |  17.0.4 |
| Input System              |  1.13.1 |
| Test Framework            |   1.4.6 |
| Timeline                  |   1.8.7 |
| Unity UI                  |   2.0.0 |
| Visual Scripting          |   1.9.5 |
| 2D Feature Set            |   2.0.1 |

---

# URP Configuration

The project is based on Unity's:

```text
Universal 2D
```

template.

The default application identifier still contains:

```text
com.DefaultCompany.2D-URP
```

while the product name is:

```text
QuizGame
```

Before publishing a build, the application identifier and company name should be changed.

---

# Installation

## Requirements

Install:

```text
Unity Hub
Unity Editor 6000.0.43f1
```

For C# development, Visual Studio or Rider can also be used.

---

# Open the Project

Clone or download the repository.

Open Unity Hub and choose:

```text
Add project from disk
```

Select the directory containing:

```text
Assets/
Packages/
ProjectSettings/
```

For this repository, that is the extracted:

```text
QuizGame_5-main
```

directory.

---

# First Import

Unity will automatically:

```text
Read Packages
      │
      ▼
Restore Unity Packages
      │
      ▼
Import Assets
      │
      ▼
Compile C# Scripts
      │
      ▼
Open Project
```

---

# Run the Game

Open:

```text
Assets/Scenes/MainMenu.unity
```

and press:

```text
Play
```

The normal gameplay sequence is:

```text
Start
  ↓
Level 1
  ↓
Level 2
  ↓
Level 3
  ↓
Final Score
  ↓
Main Menu
```

---

# Building the Game

Open Unity's build configuration and ensure these scenes are enabled:

```text
MainMenu
Level1_MultipleChoice
Level2_TrueFalse
Level3_WordGame
```

The current project already contains these scenes in the Build Settings.

You can then create a build for a supported Unity platform.

---

# Data Editing

To change the quiz without rewriting the C# scripts, edit:

```text
Assets/Resources/level1_questions.json
Assets/Resources/level2_questions.json
Assets/Resources/level3_questions.json
```

---

# Adding a Multiple-Choice Question

Use:

```json
{
  "questionText": "Which animal is the largest land mammal?",
  "options": [
    "Elephant",
    "Lion",
    "Giraffe",
    "Tiger"
  ],
  "correctAnswerIndex": 0
}
```

`correctAnswerIndex` uses zero-based indexing:

```text
0 = first option
1 = second option
2 = third option
3 = fourth option
```

---

# Adding a True / False Question

Use:

```json
{
  "questionText": "A group of lions is called a pride.",
  "isTrue": true
}
```

---

# Adding a Word Question

Use:

```json
{
  "scrambledWord": "REGIT",
  "correctWord": "TIGER"
}
```

For a proper scramble, both strings should contain exactly the same letters.

---

# Current Testing Status

The Unity package:

```text
com.unity.test-framework
```

is installed.

However, the repository does not contain project-specific automated tests for the quiz logic.

The existing TextMesh Pro example files are not application tests.

---

# Recommended Tests

Useful tests could cover:

```text
GameSession
│
├── Initial score = 0
├── Initial streak = 0
├── Score persists between scenes
└── Reset starts a new game

Level 1
│
├── Questions load from JSON
├── Correct option adds score
└── Wrong option resets streak

Level 2
│
├── True answers
├── False answers
└── Automatic Level 3 transition

Level 3
│
├── Correct word recognition
├── Case-insensitive comparison
├── Wrong answer handling
└── Final score screen
```

---

# Current Limitations

The current repository has several technical and content limitations:

1. The score is not explicitly reset when starting a new game.
2. `CorrectStreak` is also not explicitly reset when returning to the menu.
3. Several Level 3 scrambled words contain incorrect letters.
4. Two Level 3 entries are not scrambled.
5. Scene VideoPlayers reference video assets that are absent from the ZIP.
6. `GameManager1` duplicates some `GameSession` functionality.
7. There are no project-specific automated tests.
8. There is no difficulty selection.
9. Questions are always loaded in their JSON order.
10. Questions are not randomized.
11. Answer options are not randomized.
12. There is no high-score persistence.
13. There is no save system.
14. There are no player profiles.
15. There is no settings menu.
16. There is no volume control.
17. The company name remains `DefaultCompany`.
18. The application identifier is still based on the Unity template.
19. Generated editor files and logs are included in the repository.
20. No root `.gitignore` is included.
21. No standalone project `LICENSE` is included.

---

# Repository Cleanup

The repository currently includes files/directories such as:

```text
Logs/
UserSettings/
Assembly-CSharp.csproj
QuizGame.sln
```

These are generally generated or user-specific Unity/IDE files.

A normal Unity Git repository should use a `.gitignore` for directories such as:

```gitignore
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
UserSettings/
.vs/

*.csproj
*.sln
*.user
*.tmp
```

The important project directories that should remain version-controlled are:

```text
Assets/
Packages/
ProjectSettings/
```

---

# Recommended Improvements

Potential next steps include:

* reset score when Start is pressed
* correct all scrambled-word data
* restore missing background videos
* randomize question order
* randomize multiple-choice answers
* add a high-score system
* store best score with `PlayerPrefs`
* add player profiles
* add difficulty levels
* add timer-based questions
* add category selection
* add more animal questions
* add progress indicator
* add volume settings
* add pause menu
* add replay button
* consolidate `GameManager1` and `GameSession`
* add automated tests
* add a proper Unity `.gitignore`
* add a project license

---

# Suggested Improved Architecture

```text
                         Main Menu
                             │
                             ▼
                        New Game
                             │
                             ▼
                      Reset Session
                             │
                             ▼
                        GameSession
                             │
             ┌───────────────┼───────────────┐
             │               │               │
             ▼               ▼               ▼
           Score           Streak           Audio
             │
             ▼
                     Question Manager
                             │
        ┌────────────────────┼────────────────────┐
        │                    │                    │
        ▼                    ▼                    ▼
 Multiple Choice        True / False          Word Game
        │                    │                    │
        └────────────────────┼────────────────────┘
                             │
                             ▼
                        Final Score
                             │
                             ▼
                       High Score Save
```

---

# Learning Objectives

This project demonstrates:

* Unity scene management
* C# scripting
* singleton game state
* `DontDestroyOnLoad`
* JSON data loading
* Unity Resources
* TextMesh Pro
* Unity UI buttons
* TMP input fields
* coroutines
* scoring systems
* streak bonuses
* audio feedback
* scene transitions
* educational game design

---

# License

The repository currently does **not** include a standalone:

```text
LICENSE
```

file.

Therefore no explicit open-source license is currently granted for the complete project.

If the repository is intended to be open source, add an appropriate license such as MIT or another license selected by the project owner.

---

# Summary

```text
Project:          AnimalsGame / QuizGame
Engine:           Unity
Unity Version:    6000.0.43f1
Language:         C#
Rendering:        URP 17.0.4
UI:               Unity UI + TextMesh Pro
Question Format:  JSON

Levels:           3
Questions:        30

Level 1:          Multiple Choice
Level 2:          True / False
Level 3:          Word Game

Scoring:          10 points + streak bonus
Maximum Score:    735
Feedback Delay:   1.5 seconds
Final Score Time: 5 seconds

Audio:
correct.mp3
wrong.mp3

Persistent State:
Score
Correct Streak

Automated Tests:  Not implemented
License:          Not specified
```

## Final Game Flow

```text
                    Welcome to AnimalsGame
                             │
                             ▼
                           Start
                             │
                             ▼
                 Level 1 — Multiple Choice
                        10 questions
                             │
                             ▼
                  Level 2 — True / False
                        10 questions
                             │
                             ▼
                    Level 3 — Word Game
                        10 questions
                             │
                             ▼
                        Final Score
                             │
                             ▼
                         Main Menu
```

The repository provides a complete basic three-stage Unity quiz game with JSON-driven animal questions, persistent scoring, streak bonuses, audio feedback, and automatic progression between multiple quiz formats.
