
using UnityEngine;
using Systems; // Required to reference the ObstacleSpawner
using System.Collections.Generic; // Required for the Queue

namespace Core
{
    /// <summary>
    /// AUTHORITATIVE: The one and only Tile Spawner for the game.
    /// Manages tile placement and orchestrates obstacle generation for each new tile.
    /// This script acts as the central coordinator, ensuring the correct systems are called in order.
    /// </summary>
    public class MasterTileSpawner : MonoBehaviour
    {
        [Header("Core Dependencies")]
        [Tooltip("The engine that generates validated obstacle patterns.")]
        [SerializeField] private ProceduralPatternEngine _patternEngine;

        [Tooltip("The system that physically spawns obstacles based on a pattern instruction.")]
        [SerializeField] private ObstacleSpawner _obstacleSpawner;

        [Header("Tile Configuration")]
        [Tooltip("The prefab for a single ground tile. Must have a defined length.")]
        [SerializeField] private GameObject _tilePrefab;

        [Tooltip("The length of a single tile. Used to calculate spawn positions.")]
        [SerializeField] private float _tileLength = 100f;

        [Tooltip("How many tiles to keep active in the scene. Controls the view distance.")]
        [SerializeField] private int _numberOfActiveTiles = 5;

        private Queue<GameObject> _activeTiles = new Queue<GameObject>();
        private Vector3 _nextSpawnPoint = Vector3.zero;

        // Mock Game State - In a real project, this would be sourced from a GameManager
        private float _currentGameSpeed = 25f; 

        private void Start()
        {
            // --- Dependency Validation ---
            if (_patternEngine == null)
            {
                Debug.LogError("CRITICAL: MasterTileSpawner is missing a reference to the ProceduralPatternEngine. Disabling spawner.", this);
                this.enabled = false;
                return;
            }

            if (_obstacleSpawner == null)
            {
                Debug.LogError("CRITICAL: MasterTileSpawner is missing a reference to the ObstacleSpawner. Disabling spawner.", this);
                this.enabled = false;
                return;
            }

            if (_tilePrefab == null)
            {
                Debug.LogError("CRITICAL: The Tile Prefab is not assigned in MasterTileSpawner. Disabling spawner.", this);
                this.enabled = false;
                return;
            }
            
            // --- Initialization ---
            // Clear any editor-placed tiles to ensure a clean start
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Spawn the initial set of tiles to build the starting runway.
            for (int i = 0; i < _numberOfActiveTiles; i++)
            {
                // The very first tile should always be empty for a fair start.
                SpawnNewTile(isFirstTile: i == 0);
            }
        }

        /// <summary>
        /// Spawns a new tile at the end of the track and populates it with obstacles.
        /// </summary>
        /// <param name="isFirstTile">If true, the tile will be left empty.</param>
        public void SpawnNewTile(bool isFirstTile = false)
        {
            // In a production environment, this would use an object pool for performance.
            GameObject newTile = Instantiate(_tilePrefab, _nextSpawnPoint, Quaternion.identity, this.transform);
            _activeTiles.Enqueue(newTile);

            // Advance the spawn point for the next tile.
            _nextSpawnPoint.z += _tileLength;

            if (isFirstTile)
            {
                return; // Do not spawn obstacles on the first tile.
            }

            // --- ORCHESTRATION LOGIC ---
            // 1. Request a validated, deterministic pattern from the engine.
            ProceduralPattern pattern = _patternEngine.GeneratePattern(_currentGameSpeed);

            // 2. Command the ObstacleSpawner to execute the spawn instructions for that pattern.
            _obstacleSpawner.SpawnPatternOnTile(pattern, newTile.transform);
        }

        /// <summary>
        /// Recycles the oldest tile by removing it and spawning a new one to take its place.
        /// This gives the illusion of an endless world.
        /// </summary>
        public void RecycleOldestTile()
        {
            // In a production environment, the dequeued tile would be returned to an object pool.
            GameObject oldTile = _activeTiles.Dequeue();
            Destroy(oldTile);

            SpawnNewTile();
        }
    }
}
