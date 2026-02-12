using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject lowObstacle;
    public GameObject highObstacle;

    [Header("References")]
    public Transform player;

    [Header("Spawn Distances")]
    public float spawnDistance = 30f;
    public float laneDistance = 3f;

    [Header("Difficulty Settings")]
    public float startSpawnGap = 14f;
    public float minSpawnGap = 9f;
    public float difficultyRampTime = 90f;

    [Header("Wave / Rest Settings")]
    public float waveDuration = 22f;
    public float restDuration = 7f;

    private float nextSpawnZ;
    private float gameTimer;
    private float waveTimer;
    private bool isRestPhase;
    private bool canSpawn = true;

    private void Start()
    {
        ResetSpawner();
    }

    private void Update()
    {
        if (player == null || !canSpawn) return;

        gameTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;

        HandleWaveLogic();

        if (player.position.z + spawnDistance > nextSpawnZ)
        {
            if (!isRestPhase)
                SpawnObstacle();

            nextSpawnZ += GetCurrentSpawnGap();
        }
    }

    void HandleWaveLogic()
    {
        if (!isRestPhase && waveTimer >= waveDuration)
        {
            isRestPhase = true;
            waveTimer = 0f;
        }
        else if (isRestPhase && waveTimer >= restDuration)
        {
            isRestPhase = false;
            waveTimer = 0f;
        }
    }

    void SpawnObstacle()
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

    float GetCurrentSpawnGap()
    {
        float t = Mathf.Clamp01(gameTimer / difficultyRampTime);
        return Mathf.Lerp(startSpawnGap, minSpawnGap, t);
    }

    public void SetSpawning(bool value)
    {
        canSpawn = value;
    }

    public void ResetSpawner()
    {
        nextSpawnZ = player != null ? player.position.z + spawnDistance : spawnDistance;
        gameTimer = 0f;
        waveTimer = 0f;
        isRestPhase = false;
    }

    // 🛡 Future-safe helper
    bool WouldBlockAllLanes(List<int> lanes)
    {
        return lanes.Contains(-1) && lanes.Contains(0) && lanes.Contains(1);
    }
}
