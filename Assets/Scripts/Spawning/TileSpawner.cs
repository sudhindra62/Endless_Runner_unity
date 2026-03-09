
using UnityEngine;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    [Header("Tile Configuration")]
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float tileLength = 50f;
    [SerializeField] private int amountOfTilesOnScreen = 5;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        // Ensure playerTransform is assigned, otherwise find the player.
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Spawn initial tiles
        for (int i = 0; i < amountOfTilesOnScreen; i++)
        {
            SpawnTile(i == 0); // The first tile should be a safe, empty one
        }
    }

    void Update()
    {
        // Check if the player has advanced far enough to spawn a new tile
        if (playerTransform.position.z - tileLength > spawnZ - (amountOfTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    private void SpawnTile(bool isFirstTile = false)
    {
        GameObject prefabToSpawn = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        
        // For the very first tile, we might want a simple, empty one.
        // This assumes the first prefab in the array is the empty one.
        if (isFirstTile) 
        {
            prefabToSpawn = tilePrefabs[0];
        }

        GameObject tile = ObjectPool.Instance.GetObject(prefabToSpawn, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    private void DeleteTile()
    {
        // Return the oldest tile to the pool and remove it from the active list.
        ObjectPool.Instance.ReturnObject(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
