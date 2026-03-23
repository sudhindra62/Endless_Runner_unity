
using UnityEngine;

/// <summary>
/// Represents a collectible power-up object in the game world.
/// When the player collides with this object, it activates the corresponding power-up
/// via the PowerUpManager and then deactivates itself.
/// This script ensures A-to-Z connectivity from collection to effect activation.
/// </summary>
public class PowerUpObject : MonoBehaviour
{
    [Header("Power-Up Configuration")]
    [Tooltip("The power-up data associated with this object.")]
    public PowerUpDefinition powerUp;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            // Activate the power-up through the central manager.
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.ActivatePowerUp(powerUp);
            }

            // Deactivate the collectible object so it cannot be picked up again.
            gameObject.SetActive(false);
        }
    }
}
