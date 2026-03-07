using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject coinPrefab;
    public int minObstacles = 2;
    public int maxObstacles = 5;
    public int minCoins = 5;
    public int maxCoins = 10;

    void Start()
    {
        // Spawn obstacles
        int numObstacles = Random.Range(minObstacles, maxObstacles + 1);
        for (int i = 0; i < numObstacles; i++)
        {
            SpawnObstacle();
        }

        // Spawn coins
        int numCoins = Random.Range(minCoins, maxCoins + 1);
        for (int i = 0; i < numCoins; i++)
        {
            SpawnCoin();
        }
    }

    void SpawnObstacle()
    {
        // Choose a random obstacle prefab
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Choose a random lane and position
        int lane = Random.Range(-1, 2); // -1 for left, 0 for middle, 1 for right
        float xPos = lane * 2.5f;
        float zPos = Random.Range(5f, 25f);

        // Instantiate the obstacle
        Instantiate(obstaclePrefab, transform.position + new Vector3(xPos, 0.5f, zPos), Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        // Choose a random lane and position
        int lane = Random.Range(-1, 2);
        float xPos = lane * 2.5f;
        float zPos = Random.Range(5f, 25f);

        // Instantiate the coin
        Instantiate(coinPrefab, transform.position + new Vector3(xPos, 1f, zPos), Quaternion.identity, transform);
    }
}
