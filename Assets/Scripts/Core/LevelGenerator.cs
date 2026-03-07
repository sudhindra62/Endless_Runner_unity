using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] trackPrefabs;
    public int initialTracks = 10;
    public float trackLength = 30f;
    public Transform player;

    private float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < initialTracks; i++)
        {
            SpawnTrack();
        }
    }

    void Update()
    {
        if (player.position.z > spawnZ - (initialTracks * trackLength) + (trackLength * 2))
        {
            SpawnTrack();
        }
    }

    void SpawnTrack()
    {
        GameObject track = Instantiate(trackPrefabs[Random.Range(0, trackPrefabs.Length)], transform.forward * spawnZ, transform.rotation);
        spawnZ += trackLength;
    }
}
