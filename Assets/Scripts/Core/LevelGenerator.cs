
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Procedurally generates the level by spawning and managing track tiles.
/// Ensures an endless and varied path for the player.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [Header("Track Generation")]
    [SerializeField] private GameObject[] trackTiles;
    [SerializeField] private float tileLength = 50f; // Length of a single track tile prefab
    [SerializeField] private int initialTiles = 5;
    [SerializeField] private Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnPositionZ = 0;

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Spawn initial set of tiles
        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // If the player has moved far enough, spawn a new tile and remove the oldest one
        if (playerTransform.position.z - (tileLength) > spawnPositionZ - (initialTiles * tileLength))
        {
            SpawnTile();
            DeleteOldestTile();
        }
    }

    private void SpawnTile()
    {
        GameObject tileToSpawn = trackTiles[Random.Range(0, trackTiles.Length)];
        GameObject spawnedTile = Instantiate(tileToSpawn, Vector3.forward * spawnPositionZ, Quaternion.identity, this.transform);
        activeTiles.Add(spawnedTile);
        spawnPositionZ += tileLength;
    }

    private void DeleteOldestTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
