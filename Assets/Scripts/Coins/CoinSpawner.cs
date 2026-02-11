using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;

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

    void Start()
    {
        ResetSpawner();
    }

    void Update()
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

    void SpawnCoin()
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

    void SpawnSingleLane()
    {
        float xPos = activeLane * laneDistance;

        if (IsNearObstacle(xPos, nextCoinZ))
            return;

        Instantiate(coinPrefab, new Vector3(xPos, 1f, nextCoinZ), Quaternion.identity);

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

    void SpawnAllLanes()
    {
        for (int lane = -1; lane <= 1; lane++)
        {
            float xPos = lane * laneDistance;

            if (IsNearObstacle(xPos, nextCoinZ))
                continue;

            Instantiate(coinPrefab, new Vector3(xPos, 1f, nextCoinZ), Quaternion.identity);
        }
    }

    void DecideLaneMode()
    {
        multiLaneMode = Random.value < multiLaneChance;

        if (!multiLaneMode)
        {
            activeLane = Random.Range(-1, 2);
            coinsSpawnedInLane = 0;
            coinsLimitForLane = Random.Range(minCoinsPerLane, maxCoinsPerLane + 1);
        }
    }

    bool IsNearObstacle(float laneX, float coinZ)
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
