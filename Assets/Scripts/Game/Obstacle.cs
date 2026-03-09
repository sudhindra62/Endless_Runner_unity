
using UnityEngine;

/// <summary>
/// Handles the behavior of a single obstacle.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class Obstacle : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            // Check if the player has a shield
            PowerupManager powerupManager = hit.gameObject.GetComponent<PowerupManager>();
            if (powerupManager != null && powerupManager.IsShieldActive())
            {
                // Shield is active, destroy the obstacle and do nothing to the player
                Destroy(gameObject);
            } else {
                // No shield, kill the player
                GameManager.Instance.PlayerDied();
            }
        }
    }
}
