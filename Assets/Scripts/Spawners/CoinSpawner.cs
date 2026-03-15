
using UnityEngine;
using Core;

namespace Spawners
{
    public class CoinSpawner : MonoBehaviour
    {
        public int coinsToSpawn = 10;
        public float spawnRadius = 5f;

        void Start()
        {
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnCoin();
            }
        }

        void SpawnCoin()
        {
            ThemeConfig currentTheme = ThemeManager.Instance.CurrentTheme;
            if (currentTheme == null || currentTheme.coinPrefab == null)
            {
                Debug.LogWarning("Current theme does not have a coin prefab defined.");
                return;
            }

            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = transform.position.y; // Keep coins at the same height

            ObjectPooler.Instance.SpawnFromPool(currentTheme.coinPrefab.name, randomPosition, Quaternion.identity);
        }
    }
}
