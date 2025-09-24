using UnityEngine;
using TMPro;

public class ScoreManagerGame : MonoBehaviour
{
    public static ScoreManagerGame instance;
    public int score = 0;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText; // Assign this in the Inspector

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoints(int points)
    {
        score += points;

        // Clamp to 0 if score is negative
        if (score < 0)
            score = 0;

        UpdateScoreDisplay();
        Debug.Log("Current Score: " + score);
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
