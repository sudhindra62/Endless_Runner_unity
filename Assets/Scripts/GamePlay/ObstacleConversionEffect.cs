using UnityEngine;

/// <summary>
/// Manages the special effect that converts all on-screen obstacles into coins.
/// This is triggered by collecting a SpecialCollectible.
/// </summary>
public class ObstacleConversionEffect : MonoBehaviour
{
    [Header("Prefab Configuration")]
    [Tooltip("The coin prefab to instantiate when an obstacle is converted.")]
    [SerializeField] private GameObject coinPrefab;

    /// <summary>
    /// Converts all registered obstacles into coins using the ObstacleRegistry.
    /// </summary>
    public void ConvertAllObstacles()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab is not assigned in ObstacleConversionEffect.");
            return;
        }

        // Get the list of all active obstacles from the registry
        var activeObstacles = ObstacleRegistry.GetAllActiveObstacles();

        // Iterate through the obstacles and replace each one with a coin
        foreach (GameObject obstacle in activeObstacles)
        {
            if (obstacle != null)
            {
                // Instantiate a new coin at the obstacle's position and rotation
                Instantiate(coinPrefab, obstacle.transform.position, obstacle.transform.rotation);

                // Deactivate the original obstacle
                obstacle.SetActive(false);
            }
        }
    }
}
