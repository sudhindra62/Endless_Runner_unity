
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private AnimationCurve spawnRateCurve;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRateMultiplier = 1f;

    private float nextSpawnTime;
    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            float spawnRate = spawnRateCurve.Evaluate(timeElapsed) * spawnRateMultiplier;
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefabs or spawn points not set in the EnemySpawner.");
            return;
        }

        int enemyPrefabIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyPrefab = enemyPrefabs[enemyPrefabIndex];

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnPointIndex];

        ObjectPool.Instance.GetObject(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
