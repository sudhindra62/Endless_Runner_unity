using UnityEngine;

/// <summary>
/// Represents a collectible coin in the game.
/// This script has been optimized to be a simple data container, with all per-frame logic
/// removed for performance. The CoinMagnet script now handles the attraction behavior.
/// </summary>
public class Coin : MonoBehaviour
{
    [Tooltip("The score value this coin provides when collected.")]
    public int value = 1;

    private bool hasBeenCollected = false;

    /// <summary>
    /// Handles the collision with the player.
    /// Using OnTriggerEnter is efficient as it's only called on collision.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // We only care about collisions with the player.
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    /// <summary>
    /// Called when the coin is collected by the player.
    /// It adds to the score and deactivates the coin for object pooling.
    /// </summary>
    public void Collect()
    {
        // Ensure the coin can only be collected once.
        if (hasBeenCollected) return;
        hasBeenCollected = true;

        // Add score and deactivate the object.
        ScoreManager.instance.AddScore(value);
        gameObject.SetActive(false); 
    }

    /// <summary>
    /// Resets the coin's state when it is respawned from an object pool.
    /// </summary>
    private void OnEnable()
    {
        hasBeenCollected = false;
    }
}
