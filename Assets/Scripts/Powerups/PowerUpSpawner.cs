using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject shieldPrefab;

    public float spawnDistance = 35f;
    public float spawnInterval = 20f;
    public float laneDistance = 3f;

    float nextSpawn;

    void Start()
    {
        nextSpawn = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawn)
        {
            SpawnShield();
            nextSpawn = Time.time + spawnInterval;
        }
    }

    void SpawnShield()
    {
        int lane = Random.Range(-1, 2);
        float x = lane * laneDistance;
        float z = player.position.z + spawnDistance;

        Instantiate(shieldPrefab, new Vector3(x, 1f, z), Quaternion.identity);
    }
}
