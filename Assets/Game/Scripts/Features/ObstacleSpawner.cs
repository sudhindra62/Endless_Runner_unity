
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Obstacles;

public class ObstacleSpawner : MonoBehaviour
{
    private readonly List<GameObjectPool> _obstaclePools = new List<GameObjectPool>();

    public void SetObstaclePrefabs(GameObject[] prefabs)
    {
        _obstaclePools.Clear();
        foreach (var prefab in prefabs)
        {
            _obstaclePools.Add(new GameObjectPool(prefab));
        }
    }

    public void SpawnObstacle()
    {
        if (_obstaclePools.Count == 0)
        {
            return;
        }

        var pool = _obstaclePools[Random.Range(0, _obstaclePools.Count)];
        var obstacle = pool.Get();
        var obstacleComponent = obstacle.GetComponent<IObstacle>();
        obstacleComponent.SetPool(pool);
        obstacleComponent.Spawn();
    }
}
