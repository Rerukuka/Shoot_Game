using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public Text scoreText;

    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    public void AddPoint()
    {
        score++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
