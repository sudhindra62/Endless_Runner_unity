using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Generation Settings")]
    public TrackSegment[] segmentPrefabs; // An array of different segment prefabs
    public Transform playerTransform; // Reference to the player's transform
    public float spawnAheadDistance = 50f; // How far ahead of the player to spawn new segments
    public float recycleBehindDistance = 50f; // How far behind the player to recycle segments
    public int initialSegments = 5; // Number of segments to spawn at the start

    private List<TrackSegment> activeSegments = new List<TrackSegment>();
    private Vector3 nextSpawnPoint;

    // Difficulty Scaling
    private float timeAlive = 0f;
    private float difficultyMultiplier = 1f;

    void Start()
    {
        nextSpawnPoint = playerTransform.position;
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        if (playerTransform.position.z > nextSpawnPoint.z - spawnAheadDistance)
        {
            SpawnSegment();
        }

        if (activeSegments.Count > 0 && playerTransform.position.z > activeSegments[0].transform.position.z + recycleBehindDistance)
        {
            RecycleSegment();
        }

        UpdateDifficulty();
    }

    void SpawnSegment()
    {
        int randomSegmentIndex = Random.Range(0, segmentPrefabs.Length);
        TrackSegment newSegment = SegmentPoolManager.Instance.GetSegment(randomSegmentIndex);
        
        newSegment.transform.position = nextSpawnPoint;
        newSegment.transform.rotation = Quaternion.identity;

        nextSpawnPoint = newSegment.connectionPoint.position;
        activeSegments.Add(newSegment);
    }

    void RecycleSegment()
    {
        TrackSegment segmentToRecycle = activeSegments[0];
        activeSegments.RemoveAt(0);
        SegmentPoolManager.Instance.ReturnSegment(segmentToRecycle);
    }

    void UpdateDifficulty()
    {
        timeAlive += Time.deltaTime;
        difficultyMultiplier = 1 + (timeAlive / 60f); // Example: difficulty increases every 60 seconds
    }

    public float GetDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }
}
