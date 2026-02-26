
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerPowerUp powerUpManager;
    private PlayerDeathHandler deathHandler;

    private void Awake()
    {
        powerUpManager = GetComponent<PlayerPowerUp>();
        deathHandler = GetComponent<PlayerDeathHandler>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Coin"))
        {
            Coin coin = hit.gameObject.GetComponent<Coin>();
            if (coin != null)
                coin.Collect();
        }

        if (hit.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(hit.gameObject);
        }
    }

    private void HandleObstacleCollision(GameObject obstacle)
    {
        if (powerUpManager != null && powerUpManager.HasShield())
        {
            powerUpManager.BreakShield();
            Destroy(obstacle);
            return;
        }

        deathHandler?.HandleDeath();
    }
}
