
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Configuration")]
    [Tooltip("The possible X positions (lanes) for obstacles to spawn in.")]
    [SerializeField] private float[] laneXPositions = { -2f, 0f, 2f };

    public void SpawnFromPattern(ObstaclePattern pattern)
    {
        if (pattern == null || pattern.obstacles == null) return;

        float tileZPosition = transform.position.z;

        foreach (var obstacleData in pattern.obstacles)
        {
            if (obstacleData.laneIndex < 0 || obstacleData.laneIndex >= laneXPositions.Length)
            {
                Debug.LogWarning($"Invalid lane index in pattern: {obstacleData.laneIndex}");
                continue;
            }

            float spawnX = laneXPositions[obstacleData.laneIndex];
            
            // Spawn the obstacle at the beginning of the tile for simplicity.
            // A more advanced implementation could use a Z offset from the pattern data.
            Vector3 spawnPosition = new Vector3(spawnX, 0, tileZPosition);

            GameObject obstacle = ObjectPool.Instance.GetObject(obstacleData.obstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle.transform.SetParent(this.transform);
        }
    }
}
