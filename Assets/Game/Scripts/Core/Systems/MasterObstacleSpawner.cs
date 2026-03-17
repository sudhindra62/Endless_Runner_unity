using UnityEngine;

namespace EndlessRunner.Core.Systems
{
    /// <summary>
    /// The Master Obstacle Spawner is responsible for spawning obstacles based on procedural patterns.
    /// It's a singleton that can be accessed from any script.
    /// </summary>
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
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Spawns obstacles into the provided slots based on the given layout and theme.
        /// </summary>
        /// <param name="obstacleLayout">A boolean array where true indicates a slot should have an obstacle.</param>
        /// <param name="obstacleSlots">An array of transforms representing the possible spawn locations.</param>
        /// <param name="obstaclePrefabs">An array of obstacle prefabs to choose from.</param>
        public void SpawnObstaclesForPattern(bool[] obstacleLayout, Transform[] obstacleSlots, GameObject[] obstaclePrefabs)
        {
            for (int i = 0; i < obstacleLayout.Length; i++)
            {
                if (obstacleLayout[i] && i < obstacleSlots.Length)
                {
                    GameObject prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                    Instantiate(prefabToSpawn, obstacleSlots[i].position, obstacleSlots[i].rotation, obstacleSlots[i]);
                }
            }
        }
    }
}
