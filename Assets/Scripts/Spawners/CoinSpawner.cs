
using UnityEngine;
using Core;

namespace Spawners
{
    public class CoinSpawner : MonoBehaviour
    {
        public int coinsToSpawn = 10;
        public float spawnRadius = 5f;
        public GameObject coinPrefab; // Should be the same as in the ObjectPooler

        void Start()
        {
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnCoin();
            }
        }

        void SpawnCoin()
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = transform.position.y; // Keep coins at the same height

            ObjectPooler.Instance.SpawnFromPool("Coin", randomPosition, Quaternion.identity);
        }
    }
}
