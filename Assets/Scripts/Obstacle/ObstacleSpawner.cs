using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject lowObstacle;
    public GameObject highObstacle;

    [Header("References")]
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnDistance = 30f;
    public float laneDistance = 3f;
    public float initialSpawnGap = 15f;

    private float nextSpawnZ;
    private bool canSpawn = true;

    private GameDifficultyManager difficultyManager;

    private void Start()
    {
        difficultyManager = GameDifficultyManager.Instance;
        ResetSpawner();
    }

    private void Update()
    {
        if (player == null || !canSpawn)
            return;

        if (player.position.z + spawnDistance > nextSpawnZ)
        {
            if (difficultyManager == null || !difficultyManager.IsResting)
            {
                SpawnObstacle();
            }
            
            float spawnGapMultiplier = 1f;
            if (difficultyManager != null)
            {
                spawnGapMultiplier /= difficultyManager.SpawnRateMultiplier;
            }

            nextSpawnZ += initialSpawnGap * spawnGapMultiplier;
        }
    }

    private void SpawnObstacle()
    {
        int lane = Random.Range(-1, 2);
        float x = lane * laneDistance;

        GameObject prefab = Random.value > 0.5f ? lowObstacle : highObstacle;

        Instantiate(
            prefab,
            new Vector3(x, 0.5f, nextSpawnZ),
            Quaternion.identity
        );
    }

    public void SetSpawning(bool value)
    {
        canSpawn = value;
    }

    public void ResetSpawner()
    {
        nextSpawnZ = player != null
            ? player.position.z + spawnDistance
            : spawnDistance;
    }
}
