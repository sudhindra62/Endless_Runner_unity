using UnityEngine;

/// <summary>
/// Represents a special collectible item that triggers an immediate in-game effect.
/// In this case, it converts all active obstacles into coins.
/// </summary>
public class SpecialCollectible : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this collectible type.")]
    public string poolTag = "SpecialCollectible";

    public void Collect()
    {
        // Find the effect controller in the scene and execute the conversion
        ObstacleConversionEffect conversionEffect = FindFirstObjectByType<ObstacleConversionEffect>();
        if (conversionEffect != null)
        {
            conversionEffect.ConvertAllObstacles();
        }
        else
        {
            Debug.LogWarning("ObstacleConversionEffect not found in scene. Cannot convert obstacles.");
        }

        // Return the collectible to the pool instead of destroying it
        ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
    }
}
