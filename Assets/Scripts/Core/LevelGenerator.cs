
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Procedurally generates the level by spawning and recycling track segments.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class LevelGenerator : Singleton<LevelGenerator>
{
    [Header("Track Segments")]
    [SerializeField] private GameObject[] trackPrefabs;
    [SerializeField] private float trackSegmentLength = 50f;
    [SerializeField] private int initialTrackSegments = 5;
    [SerializeField] private Transform playerTransform; // Assign in inspector

    private Queue<GameObject> activeTrackSegments = new Queue<GameObject>();
    private float lastSpawnZ = 0f;

    private void Start()
    {
        if(playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerController>()?.transform;
        }

        for (int i = 0; i < initialTrackSegments; i++)
        {
            SpawnTrackSegment();
        }
    }

    private void Update()
    {
        // Check if the player has advanced far enough to spawn a new segment
        if (playerTransform.position.z > lastSpawnZ - (initialTrackSegments * trackSegmentLength) + (2 * trackSegmentLength))
        {
            SpawnTrackSegment();
            DespawnTrackSegment();
        }
    }

    private void SpawnTrackSegment()
    {
        int randomIndex = Random.Range(0, trackPrefabs.Length);
        GameObject trackPrefab = trackPrefabs[randomIndex];

        Vector3 spawnPosition = new Vector3(0, 0, lastSpawnZ);
        GameObject newSegment = ObjectPool.Instance.GetObject(trackPrefab, spawnPosition, Quaternion.identity);
        
        activeTrackSegments.Enqueue(newSegment);
        lastSpawnZ += trackSegmentLength;
    }

    private void DespawnTrackSegment()
    {
        if (activeTrackSegments.Count > initialTrackSegments)
        {
            GameObject oldSegment = activeTrackSegments.Dequeue();
            ObjectPool.Instance.ReturnObject(oldSegment);
        }
    }
}
