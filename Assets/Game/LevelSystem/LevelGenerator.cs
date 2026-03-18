using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Generation Settings")]
    public Transform playerTransform;
    public float spawnAheadDistance = 50f;
    public float recycleBehindDistance = 50f;
    public int initialSegments = 5;

    [Header("Difficulty Scaling") guesswork]
    public PlayerController playerController; 

    private GameObject[] segmentPrefabs;
    private List<TrackSegment> activeSegments = new List<TrackSegment>();
    private Vector3 nextSpawnPoint;

    // Difficulty Scaling
    private float timeAlive = 0f;
    private float difficultyMultiplier = 1f;
    
    public Action<TrackSegment> OnSegmentSpawned;

    void OnEnable()
    {
        ThemeManager.OnThemeChanged += HandleThemeChange;
    }

    void OnDisable()
    {
        ThemeManager.OnThemeChanged -= HandleThemeChange;
    }

    void Start()
    {
        nextSpawnPoint = playerTransform.position;
        if (ThemeManager.Instance != null)
        {
            HandleThemeChange(ThemeManager.Instance.CurrentConfig);
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
        if (segmentPrefabs == null || segmentPrefabs.Length == 0) return;

        int randomSegmentIndex = UnityEngine.Random.Range(0, segmentPrefabs.Length);
        TrackSegment newSegment = SegmentPoolManager.Instance.GetSegment(segmentPrefabs[randomSegmentIndex]);
        
        newSegment.transform.position = nextSpawnPoint;
        newSegment.transform.rotation = Quaternion.identity;
        
        nextSpawnPoint = newSegment.connectionPoint.position;
        activeSegments.Add(newSegment);
        
        OnSegmentSpawned?.Invoke(newSegment);
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
        difficultyMultiplier = 1 + (timeAlive / 60f); // Example: Difficulty increases every 60 seconds

        if (playerController != null)
        {
            playerController.SetSpeed(playerController.baseMoveSpeed * difficultyMultiplier);
        }
    }

    public float GetDifficultyMultiplier()
    {
        return difficultyMultiplier;
    }

    void HandleThemeChange(ThemeConfig newConfig)
    {
        if (newConfig != null)
        {
            segmentPrefabs = newConfig.segmentPrefabs;

            if (activeSegments.Count == 0)
            {
                for (int i = 0; i < initialSegments; i++)
                {
                    SpawnSegment();
                }
            }
        }
    }
}
