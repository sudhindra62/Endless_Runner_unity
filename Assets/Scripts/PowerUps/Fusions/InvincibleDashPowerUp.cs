using UnityEngine;

/// <summary>
/// The Invincible Dash fusion power-up. A combination of Shield and Speed Boost.
/// Grants invincibility, a strong forward acceleration, reduced lateral control, and a high score multiplier.
/// </summary>
public class InvincibleDashPowerUp : PowerUpEffect
{
    private PlayerController playerController;
    private PlayerCollision playerCollision;
    private ScoreManager scoreManager;

    private readonly float accelerationMultiplier;
    private readonly float lateralControlReduction;
    private readonly int scoreMultiplier;

    private float originalMaxSpeed;
    private float originalLateralSpeed;

    public InvincibleDashPowerUp(float duration, float acceleration, float lateralReduction, int multiplier) : base(duration)
    {
        this.accelerationMultiplier = acceleration;
        this.lateralControlReduction = lateralReduction;
        this.scoreMultiplier = multiplier;

        playerController = ServiceLocator.Get<PlayerController>();
        playerCollision = playerController?.GetComponent<PlayerCollision>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (playerCollision != null)
        {            
            playerCollision.SetInvincible(true);
        }

        if (playerController != null)
        {
            originalMaxSpeed = playerController.maxSpeed;
            originalLateralSpeed = playerController.laneChangeSpeed;

            playerController.maxSpeed *= accelerationMultiplier;
            playerController.laneChangeSpeed *= lateralControlReduction;
        }

        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(scoreMultiplier);
        }

        Debug.Log("Invincible Dash Activated!");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (playerCollision != null)
        {
            playerCollision.SetInvincible(false);
        }

        if (playerController != null)
        {
            playerController.maxSpeed = originalMaxSpeed;
            playerController.laneChangeSpeed = originalLateralSpeed;
        }

        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(1); // Reset to default
        }

        Debug.Log("Invincible Dash Deactivated.");
    }
}
