
/// <summary>
/// This power-up multiplies the player's score while active.
/// It activates by setting the score multiplier in the ScoreManager to 2.
/// </summary>
public class ScoreMultiplierPowerUp : PowerUpEffect
{
    private ScoreManager scoreManager;

    public ScoreMultiplierPowerUp(float duration) : base(duration)
    {
        scoreManager = ServiceLocator.Get<ScoreManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(2);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(1); // Reset to default
        }
    }
}
