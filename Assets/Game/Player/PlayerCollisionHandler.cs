using UnityEngine;

/// <summary>
/// Handles all player collision events, acting as the single authority for collision detection and response.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerDeathHandler deathHandler;
    private ScoreManager scoreManager;

    private void Awake()
    {
        deathHandler = GetComponent<PlayerDeathHandler>();
        scoreManager = ScoreManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            deathHandler.HandleDeath();
        }
        else if (other.CompareTag("Coin"))
        {
            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
            }
            other.gameObject.SetActive(false); 
        }
    }
}
