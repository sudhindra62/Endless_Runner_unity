using UnityEngine;

/// <summary>
/// Manages the spawning of bonus coins in the game world.
/// </summary>
public class BonusCoinSpawner : MonoBehaviour
{
    /// <summary>
    /// The prefab of the bonus coin to be spawned.
    /// </summary>
    public GameObject bonusCoinPrefab;

    /// <summary>
    /// The number of bonus coins to spawn at once.
    /// </summary>
    public int spawnCount = 5;

    /// <summary>
    /// The spacing between each bonus coin.
    /// </summary>
    public float coinSpacing = 2.0f;

    /// <summary>
    /// The spawn height of the bonus coins, relative to the spawner's position.
    /// </summary>
    public float spawnHeight = 1.0f;

    /// <summary>
    /// The chance (from 0 to 1) of spawning bonus coins on any given tile.
    /// </summary>
    [Range(0, 1)]
    public float spawnChance = 0.1f;

    // --- Private Fields ---
    private TileSpawner tileSpawner;

    void Start()
    {
        // Find the TileSpawner in the scene to hook into its events
        tileSpawner = FindFirstObjectByType<TileSpawner>();
        if (tileSpawner != null)
        {
            // Subscribe to the event that is triggered when a new tile is spawned
            tileSpawner.OnTileSpawned += SpawnBonusCoins;
        }
        else
        {
            Debug.LogWarning("TileSpawner not found in the scene. Bonus coins will not be spawned.");
        }
    }

    /// <summary>
    /// Called when the object is destroyed.
    /// </summary>
    void OnDestroy()
    {        // Unsubscribe from the event to prevent memory leaks
        if (tileSpawner != null)
        {
            tileSpawner.OnTileSpawned -= SpawnBonusCoins;
        }
    }

    /// <summary>
    /// Spawns bonus coins on a newly created tile based on spawn chance.
    /// </summary>
    /// <param name="tile">The tile that was just spawned.</param>
    private void SpawnBonusCoins(GameObject tile)
    {        // Only spawn if the random chance is met
        if (Random.value > spawnChance)
        {            return;
        }

        // Determine a random lane (left, middle, right)
        int lane = Random.Range(-1, 2);
        Vector3 spawnPosition = tile.transform.position + Vector3.up * spawnHeight + Vector3.right * lane * 2.0f;

        // Spawn a line of bonus coins
        for (int i = 0; i < spawnCount; i++)
        {
            if (bonusCoinPrefab != null)
            {
                Instantiate(bonusCoinPrefab, spawnPosition, Quaternion.identity, tile.transform);
                spawnPosition.z += coinSpacing;
            }
        }
    }
}
