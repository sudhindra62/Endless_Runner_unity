
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.Generation
{
    public class LevelChunk : MonoBehaviour, IPoolable
    {
        [Header("Power-Up Spawning")]
        [Range(0, 1)]
        [SerializeField] private float powerUpSpawnChance = 0.1f;

        public void OnObjectSpawn()
        {
            // Attempt to spawn a power-up when the chunk is activated.
            if (Random.value < powerUpSpawnChance)
            {
                SpawnPowerUp();
            }
        }

        private void SpawnPowerUp()
        {
            if (PowerUpManager.Instance != null)
            {
                // Choose a random position within this chunk.
                // This is a simplified example; a real implementation would be more sophisticated.
                Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1, 2), 1, Random.Range(5, 25));
                PowerUpManager.Instance.SpawnRandomPowerUp(spawnPosition, Quaternion.identity);
            }
        }
    }
}
