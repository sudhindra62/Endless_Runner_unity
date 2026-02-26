using UnityEngine;

public class ScoreInterceptor : MonoBehaviour
{
    private ScoreManager scoreManager;
    private int lastScore = 0;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            lastScore = scoreManager.GetScore();
        }
    }

    void Update()
    {
        if (scoreManager == null)
            return;

        // ADDED: Prevent null reference crash
        if (CoinDoubler.instance == null)
        {
            lastScore = scoreManager.GetScore(); // ADDED: keep sync
            return;
        }

        // ADDED: Always keep lastScore in sync when doubler inactive
        if (!CoinDoubler.instance.isDoublerActive)
        {
            lastScore = scoreManager.GetScore();
            return;
        }

        int currentScore = scoreManager.GetScore();
        if (currentScore > lastScore)
        {
            int scoreGained = currentScore - lastScore;

            // ADDED: Prevent recursive multiplication
            if (scoreGained > 0)
            {
                scoreManager.AddScore(scoreGained * (CoinDoubler.instance.multiplier - 1));
            }
        }

        lastScore = scoreManager.GetScore();
    }
}
