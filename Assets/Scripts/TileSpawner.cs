using UnityEngine;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    public GameObject roadTilePrefab;
    public Transform player;

    public int tilesOnScreen = 7;
    public float tileLength = 30f;

    private float spawnZ = 0f;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        // Spawn initial tiles
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // When player is close to the end, spawn more tiles
        if (player.position.z > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteOldTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(
            roadTilePrefab,
            Vector3.forward * spawnZ,
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    void DeleteOldTile()
    {
        if (activeTiles.Count == 0) return;

        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    // ✅ CALLED ON RESTART
    public void ResetTiles()
    {
        foreach (GameObject tile in activeTiles)
            Destroy(tile);

        activeTiles.Clear();
        spawnZ = 0f;

        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }
}
