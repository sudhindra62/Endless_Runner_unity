
using UnityEngine;
using TMPro;

/// <summary>
/// Displays the player's score and updates it in real-time.
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreDisplay;
        ScoreManager.OnHighScoreChanged += UpdateHighScoreDisplay;

        // Initial display update
        if (ScoreManager.Instance != null)
        {
            UpdateScoreDisplay(ScoreManager.Instance.CurrentScore);
            UpdateHighScoreDisplay(ScoreManager.Instance.HighScore);
        }
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreDisplay;
        ScoreManager.OnHighScoreChanged -= UpdateHighScoreDisplay;
    }

    private void UpdateScoreDisplay(long newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
        }
    }

    private void UpdateHighScoreDisplay(long newHighScore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {newHighScore}";
        }
    }
}
