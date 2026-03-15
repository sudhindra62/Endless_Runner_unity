
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Configuration")]
    [Tooltip("The possible X positions (lanes) for obstacles to spawn in.")]
    [SerializeField] private float[] laneXPositions = { -2f, 0f, 2f };

    public void SpawnFromPattern(ObstaclePattern pattern)
    {
        if (pattern == null || pattern.obstacles == null) return;

        ThemeConfig currentTheme = ThemeManager.Instance.CurrentTheme;
        if (currentTheme == null || currentTheme.obstaclePrefabs == null || currentTheme.obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Current theme does not have any obstacle prefabs defined.");
            return;
        }

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

            // Get a random obstacle prefab from the current theme
            int randomObstacleIndex = Random.Range(0, currentTheme.obstaclePrefabs.Length);
            GameObject obstaclePrefab = currentTheme.obstaclePrefabs[randomObstacleIndex];

            GameObject obstacle = ObjectPool.Instance.GetObject(obstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle.transform.SetParent(this.transform);
        }
    }
}
