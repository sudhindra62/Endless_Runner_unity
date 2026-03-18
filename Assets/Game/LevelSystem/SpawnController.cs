using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    private GameObject[] obstaclePrefabs;
    private GameObject coinPrefab;
    private GameObject[] powerUpPrefabs;

    void OnEnable()
    {
        ThemeManager.OnThemeChanged += HandleThemeChange;
        if (levelGenerator != null) levelGenerator.OnSegmentSpawned += OnSegmentSpawned;
    }

    void OnDisable()
    {
        ThemeManager.OnThemeChanged -= HandleThemeChange;
        if (levelGenerator != null) levelGenerator.OnSegmentSpawned -= OnSegmentSpawned;
    }

    void Start()
    {
        if (ThemeManager.Instance != null) HandleThemeChange(ThemeManager.Instance.CurrentConfig);
    }

    void OnSegmentSpawned(TrackSegment segment)
    {
        float difficulty = levelGenerator.GetDifficultyMultiplier();
        SpawnObstacles(segment, difficulty);
        SpawnCoins(segment, difficulty);
        SpawnPowerups(segment, difficulty);
    }

    void SpawnObstacles(TrackSegment segment, float difficulty)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        foreach (Transform obstacleSlot in segment.obstacleSlots)
        {
            if (Random.Range(0f, 1f) < (0.5f * difficulty))
            {
                int randomIndex = Random.Range(0, obstaclePrefabs.Length);
                GameObject obstacle = ObstaclePool.Instance.GetObstacle(obstaclePrefabs[randomIndex]);
                obstacle.transform.position = obstacleSlot.position;
                obstacle.transform.rotation = obstacleSlot.rotation;
                obstacle.transform.SetParent(obstacleSlot);
            }
        }
    }

    void SpawnCoins(TrackSegment segment, float difficulty)
    {
        if (coinPrefab == null) return;

        foreach (Transform coinPath in segment.coinPaths)
        {
            CoinPattern pattern = GetCoinPattern(difficulty);
            switch (pattern)
            {
                case CoinPattern.Line:
                    for (int i = 0; i < 5; i++) CreateCoin(coinPath, new Vector3(0, 0, i * 2f));
                    break;
                case CoinPattern.Curve:
                    for (int i = 0; i < 7; i++) CreateCoin(coinPath, new Vector3(Mathf.Sin(i * 0.5f) * 1.5f, 0, i * 2f));
                    break;
                case CoinPattern.ZigZag:
                    for (int i = 0; i < 8; i++) CreateCoin(coinPath, new Vector3((i % 2 == 0 ? -1 : 1) * 1.5f, 0, i * 1.5f));
                    break;
                case CoinPattern.Jump:
                     for (int i = 0; i < 3; i++) CreateCoin(coinPath, new Vector3(0, i * 0.75f, i * 2f));
                    break;
            }
        }
    }
    
    void SpawnPowerups(TrackSegment segment, float difficulty)
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;

        // Reuse obstacle slots for powerups, but with a lower probability
        foreach (Transform powerupSlot in segment.obstacleSlots)
        {
            // Ensure the slot is empty before placing a powerup
            if (powerupSlot.childCount == 0 && Random.Range(0f, 1f) < (0.1f * difficulty)) // Lower spawn chance for powerups
            {
                int randomIndex = Random.Range(0, powerUpPrefabs.Length);
                GameObject powerup = Instantiate(powerUpPrefabs[randomIndex], powerupSlot.position, powerupSlot.rotation);
                powerup.transform.SetParent(powerupSlot);
            }
        }
    }

    private CoinPattern GetCoinPattern(float difficulty)
    {
        float chance = Random.Range(0f, 1f);
        if (chance < 0.4f) return CoinPattern.Line; // Common
        if (difficulty > 1.2f && chance < 0.7f) return CoinPattern.Curve; // Medium
        if (difficulty > 1.5f && chance < 0.9f) return CoinPattern.ZigZag; // Hard
        if (difficulty > 1.8f) return CoinPattern.Jump; // Very Hard
        return CoinPattern.Line;
    }

    private void CreateCoin(Transform parent, Vector3 localPosition)
    {
        // Instantiate the theme-specific coin prefab
        GameObject coin = Instantiate(coinPrefab);
        coin.transform.SetParent(parent);
        coin.transform.localPosition = localPosition;
        coin.transform.localRotation = Quaternion.identity;
    }

    void HandleThemeChange(ThemeConfig newConfig)
    {
        if (newConfig != null)
        {
            obstaclePrefabs = newConfig.obstaclePrefabs;
            coinPrefab = newConfig.coinPrefab;
            powerUpPrefabs = newConfig.powerUpPrefabs;
        }
    }
}

public enum CoinPattern
{
    Line,
    Curve,
    ZigZag,
    Jump
}
