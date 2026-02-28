
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the spawning of obstacles on the track tiles.
/// This script works in conjunction with the TileSpawner and the GameDifficultyManager.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Configuration")]
    [Tooltip("The registry of all possible obstacles.")]
    [SerializeField] private ObstacleRegistry obstacleRegistry;

    private ObjectPooler objectPooler;
    private GameDifficultyManager difficultyManager;

    private void Start()
    {
        objectPooler = ServiceLocator.Get<ObjectPooler>();
        difficultyManager = ServiceLocator.Get<GameDifficultyManager>();

        // Initialize Object Pools for Obstacles
        foreach (var obstacleData in obstacleRegistry.allObstacles)
        {
            objectPooler.AddPool(obstacleData.name, obstacleData.prefab, 10);
        }
    }

    public void SpawnObstacles(Transform tileTransform)
    {
        List<ObstacleData> possibleObstacles = difficultyManager.GetObstaclesForCurrentDifficulty();

        // This is a simple example of how you might spawn obstacles.
        // A more advanced system would use patterns and more complex placement logic.
        for (int i = 0; i < 3; i++) // For each lane
        {
            if (Random.value < difficultyManager.GetCurrentObstacleFrequency())
            {
                ObstacleData randomObstacle = possibleObstacles[Random.Range(0, possibleObstacles.Count)];
                GameObject obstacle = objectPooler.GetPooledObject(randomObstacle.name);

                if (obstacle != null)
                {
                    // Position the obstacle on the tile in the correct lane
                    // This will need to be adjusted based on your tile and lane setup
                    float laneOffset = (i - 1) * 3f; // Example lane offset
                    obstacle.transform.position = tileTransform.position + new Vector3(laneOffset, 0, 15f);
                    obstacle.transform.rotation = Quaternion.identity;
                }
            }
        }
    }
}

[CreateAssetMenu(fileName = "ObstacleRegistry", menuName = "Endless Runner/Obstacle Registry")]
public class ObstacleRegistry : ScriptableObject
{
    public List<ObstacleData> allObstacles;
}

[System.Serializable]
public class ObstacleData
{
    public string name;
    public GameObject prefab;
    public int minDifficultyLevel;
    public int maxDifficultyLevel;
}
