
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the procedural spawning and despawning of level tiles to create an infinite runner effect.
/// It maintains a configurable number of active tiles, spawning new ones ahead of the player and despawning old ones.
/// </summary>
public class TileSpawner : MonoBehaviour
{
    [Header("Player Reference")]
    [Tooltip("The player's transform, used to calculate when to spawn new tiles. If not assigned, it will be found automatically.")]
    [SerializeField] private Transform player;

    [Header("Tile Configuration")]
    [Tooltip("The prefab for the very first tile. Should be a safe, empty tile.")]
    [SerializeField] private GameObject startTilePrefab;

    [Tooltip("A list of prefabs to be used for the procedurally generated tiles.")]
    [SerializeField] private List<GameObject> tilePrefabs = new List<GameObject>();

    [Header("Spawning Settings")]
    [Tooltip("The number of tiles to keep active in the scene at any given time.")]
    [SerializeField] private int activeTilesCount = 7;

    [Tooltip("The length of a single tile. This should be consistent for all tile prefabs.")]
    [SerializeField] private float tileLength = 30f;

    private float nextSpawnZ = 0f;
    private readonly List<GameObject> activeTiles = new List<GameObject>();

    private void Awake()
    {
        // If the player is not assigned in the Inspector, find it in the scene.
        if (player == null)
        {
            // Use FindFirstObjectByType for a more modern and efficient lookup.
            var playerMovement = FindFirstObjectByType<PlayerMovement>();
            if (playerMovement != null)
            {
                player = playerMovement.transform;
            }
            else
            {
                Debug.LogError("[TileSpawner] Player transform is not assigned and no GameObject with a PlayerMovement component was found.");
                enabled = false; // Disable the script if the player is missing.
            }
        }
    }

    private void Start()
    {
        if (startTilePrefab == null || tilePrefabs.Count == 0)
        {
            Debug.LogError("[TileSpawner] Start tile or tile prefabs are not assigned in the inspector.");
            enabled = false;
            return;
        }

        SpawnInitialTiles();
    }

    private void Update()
    {
        // Ensure the player reference is valid before proceeding.
        if (player == null) return;

        // Check if a new tile needs to be spawned based on the player's position.
        if (player.position.z - tileLength > nextSpawnZ - (activeTilesCount * tileLength))
        {
            SpawnNextTile();
            DespawnOldestTile();
        }
    }

    /// <summary>
    /// Spawns the initial set of tiles at the start of the game.
    /// </summary>
    private void SpawnInitialTiles()
    {
        SpawnTile(startTilePrefab);

        for (int i = 0; i < activeTilesCount - 1; i++)
        {
            SpawnNextTile();
        }
    }

    /// <summary>
    /// Spawns a new tile by selecting a random one from the prefabs list.
    /// </summary>
    private void SpawnNextTile()
    {
        if (tilePrefabs.Count == 0) return;

        GameObject randomTilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        SpawnTile(randomTilePrefab);
    }

    /// <summary>
    /// Instantiates a given tile prefab at the correct position and adds it to the active tiles list.
    /// </summary>
    private void SpawnTile(GameObject tilePrefab)
    {
        GameObject newTile = Instantiate(tilePrefab, Vector3.forward * nextSpawnZ, Quaternion.identity, transform);
        activeTiles.Add(newTile);
        nextSpawnZ += tileLength;
    }

    /// <summary>
    /// Removes the oldest tile from the scene.
    /// </summary>
    private void DespawnOldestTile()
    {
        if (activeTiles.Count > 0)
        {
            GameObject oldestTile = activeTiles[0];
            activeTiles.RemoveAt(0);
            Destroy(oldestTile);
        }
    }

    /// <summary>
    /// Resets the tile spawner to its initial state, clearing all tiles and respawning the starting set.
    /// </summary>
    public void ResetTiles()
    {
        foreach (var tile in activeTiles)
        {
            Destroy(tile);
        }
        activeTiles.Clear();
        nextSpawnZ = 0;

        SpawnInitialTiles();
    }
}
