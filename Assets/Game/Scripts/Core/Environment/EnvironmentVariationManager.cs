
using UnityEngine;
using System.Collections.Generic;

public class EnvironmentVariationManager : MonoBehaviour
{
    public static EnvironmentVariationManager Instance;

    [Tooltip("The parent transform for the spawned environment segments.")]
    public Transform environmentParent;

    [Tooltip("The currently active environment segments.")]
    private List<GameObject> activeSegments = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawns a new environment segment.
    /// </summary>
    /// <param name="segmentPrefab">The prefab of the segment to spawn.</param>
    /// <param name="spawnPosition">The position to spawn the segment at.</param>
    /// <param name="spawnRotation">The rotation to spawn the segment with.</param>
    public void SpawnSegment(GameObject segmentPrefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (segmentPrefab != null && environmentParent != null)
        {
            GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, spawnRotation, environmentParent);
            activeSegments.Add(newSegment);
        }
    }

    /// <summary>
    /// Despawns old environment segments.
    /// </summary>
    /// <param name="playerZPosition">The current Z position of the player.</param>
    /// <param name="despawnDistance">The distance behind the player at which segments should be despawned.</param>
    public void DespawnSegments(float playerZPosition, float despawnDistance)
    {
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i].transform.position.z < playerZPosition - despawnDistance)
            {
                Destroy(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }
}
