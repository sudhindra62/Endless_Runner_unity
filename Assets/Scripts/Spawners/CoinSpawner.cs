
using UnityEngine;


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
            ThemeSO currentTheme = ThemeManager.Instance != null ? ThemeManager.Instance.CurrentTheme : null;
            if (currentTheme == null || currentTheme.coinPrefab == null)
            {
                Debug.LogWarning("Current theme does not have a coin prefab defined.");
                return;
            }

            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = transform.position.y; // Keep coins at the same height

            ObjectPool.Instance.GetObject(currentTheme.coinPrefab, randomPosition, Quaternion.identity);
        }
    }

