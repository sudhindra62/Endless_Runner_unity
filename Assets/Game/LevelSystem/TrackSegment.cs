using UnityEngine;

namespace EndlessRunner.Level
{
    public class TrackSegment : MonoBehaviour
    {
        public GameObject prefab;
        public Transform[] spawnPoints;
        public Transform[] obstacleSlots;
        public Transform[] coinPaths;

        public void SpawnObstaclesAndCoins(ThemeConfig theme)
        {
            // Spawn obstacles
            foreach (var slot in obstacleSlots)
            {
                if (Random.value > 0.5f) // 50% chance to spawn an obstacle
                {
                    var obstaclePrefab = theme.obstaclePrefabs[Random.Range(0, theme.obstaclePrefabs.Length)];
                    Instantiate(obstaclePrefab, slot.position, slot.rotation, slot);
                }
            }

            // Spawn coins
            foreach (var path in coinPaths)
            {
                SpawnCoinsOnPath(path, theme.coinPrefab);
            }
        }

        private void SpawnCoinsOnPath(Transform path, GameObject coinPrefab)
        {
            var distance = 0f;
            var pathLength = GetPathLength(path);
            while (distance < pathLength)
            {
                var coin = CoinPool.Instance.GetCoin();
                coin.transform.position = GetPointOnPath(path, distance);
                distance += 2f; // Spawn a coin every 2 meters
            }
        }

        private float GetPathLength(Transform path)
        {
            var length = 0f;
            for (int i = 0; i < path.childCount - 1; i++)
            {
                length += Vector3.Distance(path.GetChild(i).position, path.GetChild(i + 1).position);
            }
            return length;
        }

        private Vector3 GetPointOnPath(Transform path, float distance)
        {
            var currentDistance = 0f;
            for (int i = 0; i < path.childCount - 1; i++)
            {
                var segmentLength = Vector3.Distance(path.GetChild(i).position, path.GetChild(i + 1).position);
                if (currentDistance + segmentLength >= distance)
                {
                    var t = (distance - currentDistance) / segmentLength;
                    return Vector3.Lerp(path.GetChild(i).position, path.GetChild(i + 1).position, t);
                }
                currentDistance += segmentLength;
            }
            return path.GetChild(path.childCount - 1).position;
        }

        public Transform GetNextSpawnPoint()
        {
            if (spawnPoints.Length > 0)
            {
                return spawnPoints[Random.Range(0, spawnPoints.Length)];
            }
            return transform;
        }
    }
}
