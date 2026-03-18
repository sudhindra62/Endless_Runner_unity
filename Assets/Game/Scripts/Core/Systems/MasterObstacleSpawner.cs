using UnityEngine;

namespace EndlessRunner.Core.Systems
{
    public class MasterObstacleSpawner : MonoBehaviour
    {
        public static MasterObstacleSpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void SpawnObstaclesForPattern(int[] obstacleLayout, Transform[] obstacleSlots, GameObject[] obstaclePrefabs)
        {
            for (int i = 0; i < obstacleLayout.Length; i++)
            {
                if (obstacleLayout[i] != 0)
                {
                    int prefabIndex = obstacleLayout[i] - 1;
                    if (prefabIndex >= 0 && prefabIndex < obstaclePrefabs.Length)
                    {
                        Instantiate(obstaclePrefabs[prefabIndex], obstacleSlots[i].position, obstacleSlots[i].rotation);
                    }
                }
            }
        }
    }
}
