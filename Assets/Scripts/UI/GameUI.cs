
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    void Start()
    {
        if (ScoringManager.instance != null)
        {
            ScoringManager.instance.scoreText = scoreText;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + HighScoreManager.GetHighScore();
        }
    }
}
