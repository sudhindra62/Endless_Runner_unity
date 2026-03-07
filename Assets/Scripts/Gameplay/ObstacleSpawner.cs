
using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [SerializeField] public Transform player;
    [SerializeField] public float spawnDistance = 30f;
    [SerializeField] public float spawnInterval = 2f;
    [SerializeField] public float laneDistance = 3f;

    private float nextSpawnTime;
    private Vector3 spawnPosition;
    private List<int> lanes = new List<int> { -1, 0, 1 };

    void Update()
    {
        if (player != null && Time.time > nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObstacle()
    {
        int randomLane = lanes[Random.Range(0, lanes.Count)];
        spawnPosition = new Vector3(randomLane * laneDistance, 0.5f, player.position.z + spawnDistance);
        
        GameObject obstacle = ObjectPool.Instance.GetPooledObject();
        if (obstacle != null)
        {
            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = Quaternion.identity;
            obstacle.SetActive(true);
        }
    }
}
