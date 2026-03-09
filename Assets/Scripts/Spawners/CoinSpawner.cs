
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public enum CoinPattern { Line, Curve, Wave }

    [Header("Coin Configuration")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private CoinPattern pattern = CoinPattern.Line;
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

        float spawnX = laneXPositions[Random.Range(0, laneXPositions.Length)];
        float startZ = transform.position.z + Random.Range(10f, 30f);

        switch (pattern)
        {
            case CoinPattern.Line:
                SpawnLinePattern(spawnX, startZ);
                break;
            case CoinPattern.Curve:
                SpawnCurvePattern(spawnX, startZ);
                break;
            case CoinPattern.Wave:
                SpawnWavePattern(spawnX, startZ);
                break;
        }
    }

    private void SpawnLinePattern(float spawnX, float startZ)
    {
        for (int i = 0; i < coinsInPattern; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnX, 1f, startZ + (i * coinSpacing));
            GameObject coin = ObjectPool.Instance.GetObject(coinPrefab, spawnPosition, Quaternion.identity);
            coin.transform.SetParent(transform);
        }
    }

    private void SpawnCurvePattern(float spawnX, float startZ)
    {
        for (int i = 0; i < coinsInPattern; i++)
        {
            float xOffset = Mathf.Sin(i * 0.5f) * 2f;
            Vector3 spawnPosition = new Vector3(spawnX + xOffset, 1f, startZ + (i * coinSpacing));
            GameObject coin = ObjectPool.Instance.GetObject(coinPrefab, spawnPosition, Quaternion.identity);
            coin.transform.SetParent(transform);
        }
    }

    private void SpawnWavePattern(float spawnX, float startZ)
    {
        for (int i = 0; i < coinsInPattern; i++)
        {
            float xOffset = Mathf.Sin(i * coinSpacing) * 2f;
            Vector3 spawnPosition = new Vector3(spawnX + xOffset, 1f, startZ + (i * coinSpacing));
            GameObject coin = ObjectPool.Instance.GetObject(coinPrefab, spawnPosition, Quaternion.identity);
            coin.transform.SetParent(transform);
        }
    }
}
