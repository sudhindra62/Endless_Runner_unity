using UnityEngine;

/// <summary>
/// Represents a special collectible item that triggers an immediate in-game effect.
/// In this case, it converts all active obstacles into coins.
/// </summary>
public class SpecialCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Ensure the collision is with the player
        if (other.CompareTag("Player"))
        {
            // Find the effect controller in the scene and execute the conversion
            ObstacleConversionEffect conversionEffect = FindObjectOfType<ObstacleConversionEffect>();
            if (conversionEffect != null)
            {
                conversionEffect.ConvertAllObstacles();
            }
            else
            {
                Debug.LogWarning("ObstacleConversionEffect not found in scene. Cannot convert obstacles.");
            }

            // Remove the collectible from the scene after it has been collected
            Destroy(gameObject);
        }
    }
}
