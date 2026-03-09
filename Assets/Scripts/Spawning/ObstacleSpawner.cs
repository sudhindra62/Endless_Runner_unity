
using UnityEngine;

/// <summary>
/// Manages the spawning of obstacles within the level.
/// This script is designed to be placed on track tile prefabs.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Configuration")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [Tooltip("The number of obstacles to attempt to spawn on this tile.")]
    [SerializeField] private int obstacleCount = 3;
    [Tooltip("The possible X positions (lanes) for obstacles to spawn in.")]
    [SerializeField] private float[] laneXPositions = { -2f, 0f, 2f };

    void Start()
    {
        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        if (obstaclePrefabs.Length == 0) return;

        // Create a list of available lane indices to prevent spawning multiple obstacles in the same lane
        List<int> availableLaneIndices = new List<int>();
        for (int i = 0; i < laneXPositions.Length; i++)
        {
            availableLaneIndices.Add(i);
        }

        for (int i = 0; i < obstacleCount; i++)
        {
            if (availableLaneIndices.Count == 0) break; // No more lanes to spawn in

            // Choose a random lane
            int laneIndex = availableLaneIndices[Random.Range(0, availableLaneIndices.Count)];
            availableLaneIndices.Remove(laneIndex);

            float spawnX = laneXPositions[laneIndex];
            // Spawn position along the Z-axis of the tile (e.g., from 10% to 90% of the tile length)
            float spawnZ = transform.position.z + Random.Range(5f, 45f);

            Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);

            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }
}
