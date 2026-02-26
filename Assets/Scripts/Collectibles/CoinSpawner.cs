using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Unified CoinSpawner
/// Preserves:
/// - Lane logic
/// - ZigZag pattern
/// - Multi-lane mode
/// - Obstacle safety
/// - Reset logic
/// - Pooling optimization
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;

    [Header("Pooling")]
    public int poolSize = 100;
    private List<GameObject> coinPool;

    [Header("Lane Settings")]
    public float laneDistance = 3f;

    [Header("Coin Spacing")]
    public float coinSpacing = 4.5f;
    public int minCoinsPerLane = 5;
    public int maxCoinsPerLane = 10;

    [Header("Spawn Distance")]
    public float spawnAheadDistance = 30f;

    [Header("Obstacle Safety")]
    public float obstacleSafeGap = 7f;

    [Header("Lane Logic")]
    public float laneHoldDistance = 15f;
    public float multiLaneChance = 0.15f;

    [Header("ZigZag")]
    public int zigZagEveryCoins = 3;

    private float nextCoinZ;
    private float nextLaneSwitchZ;
    private int activeLane;
    private bool multiLaneMode;

    private int coinsSpawnedInLane;
    private int coinsLimitForLane;

    private int zigZagCounter;
    private int zigZagDirection;

    private void Start()
    {
        CreateCoinPool();
        ResetSpawner();
    }

    private void CreateCoinPool()
    {
        coinPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coinPool.Add(coin);
        }
    }

  private GameObject GetPooledCoin()
{
    for (int i = 0; i < coinPool.Count; i++)
    {
        if (!coinPool[i].activeInHierarchy)
            return coinPool[i];
    }

    // SAFE EXPANSION (only when needed)
    GameObject coin = Instantiate(coinPrefab);
    coin.SetActive(false);
    coinPool.Add(coin);

    return coin;
}

    private void Update()
    {
        if (!player || !coinPrefab) return;

        if (player.position.z + spawnAheadDistance > nextCoinZ)
        {
            SpawnCoin();
            nextCoinZ += coinSpacing;
        }

        if (player.position.z > nextLaneSwitchZ)
        {
            DecideLaneMode();
            nextLaneSwitchZ = player.position.z + laneHoldDistance;
        }
    }

    private void SpawnCoin()
    {
        if (multiLaneMode)
        {
            SpawnAllLanes();
        }
        else
        {
            if (coinsSpawnedInLane >= coinsLimitForLane)
                return;

            SpawnSingleLane();
            coinsSpawnedInLane++;
        }
    }

    private void SpawnSingleLane()
    {
        float xPos = activeLane * laneDistance;

        if (IsNearObstacle(xPos, nextCoinZ))
            return;

        GameObject coin = GetPooledCoin();

        if (coin != null)
        {
            coin.transform.position = new Vector3(xPos, 1f, nextCoinZ);
            coin.SetActive(true);
        }

        zigZagCounter++;

        if (zigZagCounter >= zigZagEveryCoins)
        {
            activeLane += zigZagDirection;

            if (activeLane > 1 || activeLane < -1)
            {
                zigZagDirection *= -1;
                activeLane = Mathf.Clamp(activeLane, -1, 1);
            }

            zigZagCounter = 0;
        }
    }

    private void SpawnAllLanes()
    {
        for (int lane = -1; lane <= 1; lane++)
        {
            float xPos = lane * laneDistance;

            if (IsNearObstacle(xPos, nextCoinZ))
                continue;

            GameObject coin = GetPooledCoin();

            if (coin != null)
            {
                coin.transform.position = new Vector3(xPos, 1f, nextCoinZ);
                coin.SetActive(true);
            }
        }
    }

    private void DecideLaneMode()
    {
        multiLaneMode = Random.value < multiLaneChance;

        if (!multiLaneMode)
        {
            activeLane = Random.Range(-1, 2);
            coinsSpawnedInLane = 0;
            coinsLimitForLane = Random.Range(minCoinsPerLane, maxCoinsPerLane + 1);
        }
    }

    private bool IsNearObstacle(float laneX, float coinZ)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obs in obstacles)
        {
            if (Mathf.Abs(obs.transform.position.x - laneX) < 0.5f)
            {
                if (Mathf.Abs(obs.transform.position.z - coinZ) < obstacleSafeGap)
                    return true;
            }
        }

        return false;
    }

    public void ResetSpawner()
    {
        if (!player) return;

        nextCoinZ = player.position.z + spawnAheadDistance;
        nextLaneSwitchZ = player.position.z;

        activeLane = Random.Range(-1, 2);
        multiLaneMode = false;

        coinsSpawnedInLane = 0;
        coinsLimitForLane = Random.Range(minCoinsPerLane, maxCoinsPerLane + 1);

        zigZagCounter = 0;
        zigZagDirection = Random.value > 0.5f ? 1 : -1;
    }
}
