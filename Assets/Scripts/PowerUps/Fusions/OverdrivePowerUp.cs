using UnityEngine;

/// <summary>
/// The Overdrive fusion power-up. This is a combination of the Speed Boost and Score Multiplier.
/// It grants a major speed increase and a high score multiplier.
/// </summary>
public class OverdrivePowerUp : PowerUpEffect
{
    private GameDifficultyManager difficultyManager;
    private ScoreManager scoreManager;

    private readonly float speedMultiplier;
    private readonly int scoreMultiplier;

    public OverdrivePowerUp(float duration, float speedMult, int scoreMult) : base(duration)
    {
        this.speedMultiplier = speedMult;
        this.scoreMultiplier = scoreMult;
        difficultyManager = ServiceLocator.Get<GameDifficultyManager>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (difficultyManager != null)
        {
            difficultyManager.SetSpeedBoostMultiplier(speedMultiplier);
        }

        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(scoreMultiplier);
        }

        Debug.Log($"Overdrive Activated! Speed: {speedMultiplier}x, Score: {scoreMultiplier}x.");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (difficultyManager != null)
        {
            difficultyManager.SetSpeedBoostMultiplier(1.0f); // Reset to default
        }

        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(1); // Reset to default
        }

        Debug.Log("Overdrive Deactivated.");
    }
}
