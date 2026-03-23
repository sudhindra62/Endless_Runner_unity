
using System.Collections;
using UnityEngine;

    public class PowerUpSpawner : MonoBehaviour
    {
        public GameObject[] powerUpPrefabs;
        public float spawnRadius = 5f;
        public float spawnInterval = 10f;

        void Start()
        {
            StartCoroutine(SpawnPowerUpsPeriodically());
        }

        IEnumerator SpawnPowerUpsPeriodically()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnPowerUp();
            }
        }

        void SpawnPowerUp()
        {
            if (powerUpPrefabs.Length == 0)
            {
                Debug.LogWarning("No power-up prefabs assigned to the PowerUpSpawner.");
                return;
            }

            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject powerUpPrefab = powerUpPrefabs[randomIndex];

            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 1;

            Instantiate(powerUpPrefab, randomPosition, Quaternion.identity);
        }
    }
