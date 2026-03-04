using UnityEngine;

/// <summary>
/// Handles all collision and trigger interactions for the player.
/// This script is responsible for detecting damage, notifying analytics of dodges,
/// and initiating the death sequence.
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    private bool inDodgeTrigger = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Assuming all deadly obstacles have the "Obstacle" tag.
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleDeath(collision.gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // The "DodgeTrigger" is a volume in front of an obstacle that warns the player.
        if (other.CompareTag("DodgeTrigger"))
        {
            inDodgeTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DodgeTrigger"))
        {
            // If the player exits the trigger, it means they successfully dodged.
            if (inDodgeTrigger)
            {
                // --- ANALYTICS INTEGRATION ---
                if (PlayerAnalyticsManager.Instance != null)
                {
                    PlayerAnalyticsManager.Instance.LogDodgeSuccess();
                }
                inDodgeTrigger = false; // Reset for the next dodge.
            }
        }
    }

    private void HandleDeath(string cause)
    {
        // If the player dies while in a dodge trigger, it's a failed dodge.
        if (inDodgeTrigger)
        {
            // --- ANALYTICS INTEGRATION ---
            if (PlayerAnalyticsManager.Instance != null)
            {
                PlayerAnalyticsManager.Instance.LogDodgeFail();
            }
            inDodgeTrigger = false; // Reset
        }

        // --- ANALYTICS INTEGRATION ---
        // Log the cause of death regardless of dodge status.
        if (PlayerAnalyticsManager.Instance != null)
        {
            PlayerAnalyticsManager.Instance.LogDeath(cause);
        }

        // Notify the GameFlowController to end the game.
        if (GameFlowController.Instance != null)
        {
            GameFlowController.Instance.EndGame(false); // Player did not win.
        }

        // Disable the player object.
        gameObject.SetActive(false);
        Debug.Log($"Player has been disabled by death. Cause: {cause}");
    }
}
