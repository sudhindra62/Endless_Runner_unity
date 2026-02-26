
using UnityEngine;
using System.Collections;

/// <summary>
/// An isolated spawner for placing special collectibles in the game world.
/// It operates on its own timer and does not interact with the main obstacle spawner.
/// This ensures the new collectibles can be added without modifying existing gameplay logic.
/// 
/// --- Inspector Setup ---
/// 1. Attach this to a new GameObject in your game scene.
/// 2. Assign the 'SpecialCollectiblePrefab' to be spawned.
/// 3. Define the spawn area using 'spawnCenter' and 'spawnSize'.
/// 4. Set the spawn timing parameters.
/// </summary>
public class SpecialCollectibleSpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [SerializeField] private GameObject specialCollectiblePrefab;
    [SerializeField] private float initialSpawnDelay = 10f;
    [SerializeField] private float minSpawnInterval = 15f;
    [SerializeField] private float maxSpawnInterval = 30f;

    [Header("Spawn Area")]
    [Tooltip("The center of the rectangular area where collectibles can spawn.")]
    [SerializeField] private Vector3 spawnCenter = new Vector3(0, 1, 10);
    [Tooltip("The size of the rectangular area where collectibles can spawn.")]
    [SerializeField] private Vector3 spawnSize = new Vector3(5, 2, 0); // X, Y, Z

    private bool isSpawning = false;

    private void Start()
    {
        if (specialCollectiblePrefab != null)
        {
            StartCoroutine(SpawnRoutine());
            isSpawning = true;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (isSpawning)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnCollectible();
        }
    }

    private void SpawnCollectible()
    {
        Vector3 spawnPos = spawnCenter + new Vector3(
            Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
            Random.Range(-spawnSize.y / 2, spawnSize.y / 2),
            Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
        );

        Instantiate(specialCollectiblePrefab, spawnPos, Quaternion.identity, this.transform);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the spawn area in the editor
        Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.5f);
        Gizmos.DrawCube(spawnCenter, spawnSize);
    }
}
