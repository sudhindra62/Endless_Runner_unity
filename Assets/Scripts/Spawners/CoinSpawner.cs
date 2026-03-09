
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the spawning of coins on track tiles, often in patterns.
/// This script is designed to be placed on track tile prefabs.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Configuration")]
    [SerializeField] private GameObject coinPrefab;
    [Tooltip("The number of coins to spawn in a single pattern.")]
    [SerializeField] private int coinsInPattern = 5;
    [Tooltip("The spacing between coins in a pattern.")]
    [SerializeField] private float coinSpacing = 2f;
    [Tooltip("The possible X positions (lanes) for coin patterns to spawn in.")]
    [SerializeField] private float[] laneXPositions = { -2f, 0f, 2f };

    void Start()
    {
        SpawnCoinPattern();
    }

    private void SpawnCoinPattern()
    {
        if (coinPrefab == null) return;

        // Choose a random lane for the coin pattern
        float spawnX = laneXPositions[Random.Range(0, laneXPositions.Length)];
        // Choose a random starting Z position for the pattern
        float startZ = transform.position.z + Random.Range(10f, 30f);

        for (int i = 0; i < coinsInPattern; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnX, 1f, startZ + (i * coinSpacing));
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }
}
