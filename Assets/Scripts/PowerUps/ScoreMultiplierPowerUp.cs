using UnityEngine;

/// <summary>
/// This power-up multiplies the player's score while active.
/// It activates by setting the score multiplier in the ScoreManager to 2.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/ScoreMultiplier")]
public class ScoreMultiplierPowerUp : PowerUp
{
    public int Multiplier = 2;
    
    public override void Activate(GameObject player)
    {
        ScoreManager scoreManager = ServiceLocator.Get<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(Multiplier);
        }
    }

    public override void Deactivate(GameObject player)
    {
        ScoreManager scoreManager = ServiceLocator.Get<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier(1);
        }
    }
}
