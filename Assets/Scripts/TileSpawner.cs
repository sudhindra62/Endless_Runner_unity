using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Unified TileSpawner
/// Preserves:
/// - Tile spawning logic
/// - Reset functionality
/// - Event hook for extensions
/// - Null safety
/// </summary>
public class TileSpawner : MonoBehaviour
{
    public event System.Action<GameObject> OnTileSpawned;

    [Header("References")]
    public GameObject roadTilePrefab;
    public Transform player;

    [Header("Tile Settings")]
    public int tilesOnScreen = 7;
    public float tileLength = 30f;

    private float spawnZ = 0f;
    private readonly List<GameObject> activeTiles = new List<GameObject>();

    private void Start()
    {
        SpawnInitialTiles();
    }

    private void Update()
    {
        if (player == null) return;

        if (player.position.z > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteOldTile();
        }
    }

    private void SpawnInitialTiles()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        GameObject tile = Instantiate(
            roadTilePrefab,
            Vector3.forward * spawnZ,
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;

        OnTileSpawned?.Invoke(tile);
    }

    private void DeleteOldTile()
    {
        if (activeTiles.Count == 0) return;

        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    // 🔁 Called on restart
    public void ResetTiles()
    {
        foreach (GameObject tile in activeTiles)
            Destroy(tile);

        activeTiles.Clear();
        spawnZ = 0f;

        SpawnInitialTiles();
    }
}
