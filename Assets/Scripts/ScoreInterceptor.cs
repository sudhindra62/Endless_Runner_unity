
using UnityEngine;

public class ScoreInterceptor : MonoBehaviour
{
    private ScoreManager scoreManager;
    private int lastScore = 0;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            lastScore = scoreManager.GetScore();
        }
    }

    void Update()
    {
        if (scoreManager == null || !CoinDoubler.instance.isDoublerActive)
        {
            return;
        }

        int currentScore = scoreManager.GetScore();
        if (currentScore > lastScore)
        {
            int scoreGained = currentScore - lastScore;
            scoreManager.AddScore(scoreGained * (CoinDoubler.instance.multiplier - 1));
        }

        lastScore = scoreManager.GetScore();
    }
}
