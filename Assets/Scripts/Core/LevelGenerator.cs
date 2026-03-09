using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Procedurally generates the level by spawning and recycling track segments in sync with the game state.
/// Logic restored and fortified by Supreme Guardian Architect v12.
/// This system ensures an endless and varied track for the player to traverse.
/// </summary>
public class LevelGenerator : Singleton<LevelGenerator>
{
    [Header("Track Assets")]
    [Tooltip("An array of track segment prefabs to be used for generation.")]
    [SerializeField] private GameObject[] trackPrefabs;

    [Header("Generation Settings")]
    [Tooltip("The length of a single track segment. This must be consistent for all prefabs.")]
    [SerializeField] private float trackSegmentLength = 50f;
    [Tooltip("The number of track segments to instantiate at the start and maintain ahead of the player.")]
    [SerializeField] private int segmentsToMaintain = 5;

    // --- PRIVATE STATE ---
    private Transform _playerTransform;
    private Queue<GameObject> _activeTrackSegments = new Queue<GameObject>();
    private float _lastSpawnZ = 0f;
    private bool _isGenerating = false;

    // --- UNITY LIFECYCLE & EVENT HANDLING ---

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to the GameManager to control generation based on game state. ---
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks. ---
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    void Update()
    {
        if (!_isGenerating || _playerTransform == null) return;

        // Proactively spawn new track segments well ahead of the player.
        if (_playerTransform.position.z > _lastSpawnZ - (segmentsToMaintain * trackSegmentLength))
        {
            SpawnTrackSegment();
        }

        // Continuously clean up segments that are far behind the player.
        // This is more robust than simply maintaining a fixed number of segments.
        if (_activeTrackSegments.Count > 0)
        {
            GameObject oldestSegment = _activeTrackSegments.Peek();
            if (oldestSegment != null && _playerTransform.position.z > oldestSegment.transform.position.z + (trackSegmentLength * 1.5f))
            {
                GameObject segmentToDespawn = _activeTrackSegments.Dequeue();
                ObjectPool.Instance.ReturnObject(segmentToDespawn);
            }
        }
    }

    // --- GAME STATE INTEGRATION ---

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            if (!_isGenerating) // Only start a new generation cycle if one isn't already running.
            {
                InitializeLevel();
            }
        }
        else if (newState == GameState.MainMenu || newState == GameState.GameOver)
        {
            // Stop generation and clean up the level when the game is not active.
            StopAndResetLevel();
        }
    }

    /// <summary>
    /// Initializes the entire level generation process.
    /// </summary>
    private void InitializeLevel()
    {
        // --- CONTEXT_WIRING: Find the player transform dynamically. ---
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Guardian Architect Error: LevelGenerator could not find a target with the 'Player' tag. Generation cannot start.");
            return;
        }
        _playerTransform = playerObject.transform;

        // Pre-warm the level with the initial set of track segments.
        _lastSpawnZ = 0f; // Reset spawn position
        for (int i = 0; i < segmentsToMaintain; i++)
        {
            SpawnTrackSegment();
        }

        _isGenerating = true;
    }

    /// <summary>
    /// Stops generation and returns all active track segments to the object pool.
    /// </summary>
    private void StopAndResetLevel()
    {
        _isGenerating = false;
        while (_activeTrackSegments.Count > 0)
        {
            GameObject segment = _activeTrackSegments.Dequeue();
            ObjectPool.Instance.ReturnObject(segment);
        }
        _lastSpawnZ = 0f;
    }

    // --- SEGMENT SPAWNING ---

    private void SpawnTrackSegment()
    {
        if (trackPrefabs.Length == 0)
        {
            Debug.LogError("Guardian Architect Error: No track prefabs assigned in the LevelGenerator.");
            return;
        }

        // Select a random prefab from the list.
        int randomIndex = Random.Range(0, trackPrefabs.Length);
        GameObject trackPrefab = trackPrefabs[randomIndex];

        // Get a segment from the object pool and position it correctly.
        Vector3 spawnPosition = new Vector3(0, 0, _lastSpawnZ);
        GameObject newSegment = ObjectPool.Instance.GetObject(trackPrefab, spawnPosition, Quaternion.identity);

        // Add the new segment to our tracking queue and update the spawn position for the next one.
        _activeTrackSegments.Enqueue(newSegment);
        _lastSpawnZ += trackSegmentLength;
    }
}
