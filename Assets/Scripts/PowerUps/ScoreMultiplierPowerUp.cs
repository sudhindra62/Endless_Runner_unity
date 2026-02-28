using UnityEngine;

public class ScoreMultiplierPowerUp : PowerUp
{
    private ScoreManager scoreManager;

    [Header("Configuration")]
    [SerializeField] private int scoreMultiplier = 2;

    public override void ApplyPowerUp()
    {
        base.ApplyPowerUp();
        scoreManager = ServiceLocator.Get<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(scoreMultiplier);
        }
    }

    public override void RemovePowerUp()
    {
        base.RemovePowerUp();
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(1);
        }
    }
}
