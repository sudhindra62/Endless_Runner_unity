using UnityEngine;


    /// <summary>
    /// A centralized system for spawning coins, designed to work with procedural patterns.
    /// </summary>
    public class CoinSystem : MonoBehaviour
    {
        public static CoinSystem Instance { get; private set; }

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
        /// Spawns coins along the given paths based on the provided layout.
        /// </summary>
        /// <param name="coinLayout">A boolean array indicating where to spawn coins.</param>
        /// <param name="coinPaths">An array of transforms representing the paths for the coins.</param>
        /// <param name="coinPrefab">The coin prefab to instantiate.</param>
        public void SpawnCoinsForPattern(bool[] coinLayout, Transform[] coinPaths, GameObject coinPrefab)
        {
            for (int i = 0; i < coinLayout.Length; i++)
            {
                if (coinLayout[i] && i < coinPaths.Length)
                {
                    // Here you could implement more complex logic for coin placement along the path.
                    // For now, I'm just placing a single coin at the start of the path.
                    Instantiate(coinPrefab, coinPaths[i].position, coinPaths[i].rotation, coinPaths[i]);
                }
            }
        }
        public void SpawnCoins(Transform path, GameObject coinPrefab)
        {
            // This method is kept for backwards compatibility or for simpler coin spawning scenarios.
            // A more advanced implementation might use this for specific power-up effects or other game mechanics.

            // For this example, we will spawn a simple line of coins along the path.
            // The path is assumed to be a transform that represents the starting point and direction of the coin path.

            // The number of coins to spawn in a single path.
            const int coinsToSpawn = 5;

            // The distance between each spawned coin.
            const float coinSpacing = 2.0f;

            if (coinPrefab != null && path != null)
            {
                for (int i = 0; i < coinsToSpawn; i++)
                {
                    Vector3 spawnPosition = path.position + path.forward * (i * coinSpacing);
                    Instantiate(coinPrefab, spawnPosition, Quaternion.identity, path);
                }
            }
        }

    }

