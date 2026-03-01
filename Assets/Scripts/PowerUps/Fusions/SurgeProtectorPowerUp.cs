using UnityEngine;

/// <summary>
/// The Surge Protector fusion power-up. This is a combination of the Shield and Score Multiplier.
/// It grants invincibility and a high score multiplier.
/// </summary>
public class SurgeProtectorPowerUp : PowerUpEffect
{
    private PlayerCollision playerCollision;
    private ScoreManager scoreManager;

    private readonly int scoreMultiplier;

    public SurgeProtectorPowerUp(float duration, int multiplier) : base(duration)
    {
        this.scoreMultiplier = multiplier;
        playerCollision = ServiceLocator.Get<PlayerController>()?.GetComponent<PlayerCollision>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (playerCollision != null)
        {
            playerCollision.SetInvincible(true);
        }

        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(scoreMultiplier);
        }

        Debug.Log($"Surge Protector Activated! Invincible with a {scoreMultiplier}x score multiplier.");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (playerCollision != null)
        {
            playerCollision.SetInvincible(false);
        }

        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(1); // Reset to default
        }

        Debug.Log("Surge Protector Deactivated.");
    }
}
