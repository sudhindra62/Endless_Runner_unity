
using EndlessRunner.Core;
using EndlessRunner.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Level
{
    /// <summary>
    /// Procedurally generates the level by spawning and managing level segments.
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Level Configuration")]
        [Tooltip("An array of level segment prefabs to be used for generation.")]
        [SerializeField] private GameObject[] levelSegments;

        [Tooltip("The number of level segments to spawn at the beginning of the game.")]
        [SerializeField] private int initialSegments = 5;

        [Tooltip("The length of a single level segment. This should be consistent across all prefabs.")]
        [SerializeField] private float segmentLength = 20f;

        [Header("Player Reference")]
        [Tooltip("A reference to the player's transform to track their position.")]
        [SerializeField] private Transform playerTransform;

        private List<GameObject> activeSegments = new List<GameObject>();
        private float spawnPosition = 0f;
        private GameManager gameManager;

        /// <summary>
        /// Subscribes to game state changes and initializes the level.
        /// </summary>
        private void Start()
        {
            gameManager = ServiceLocator.Get<GameManager>();
            gameManager.OnGameStateChanged += OnGameStateChanged;
            InitializeLevel();
        }

        /// <summary>
        /// Unsubscribes from game state changes to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
            }
        }

        /// <summary>
        /// Checks if a new segment needs to be spawned based on the player's position.
        /// </summary>
        private void Update()
        {
            if (gameManager.CurrentGameState != GameManager.GameState.Playing) return;

            // Spawn a new segment and remove the oldest one when the player has moved far enough
            if (playerTransform.position.z - segmentLength > spawnPosition - (initialSegments * segmentLength))
            {
                SpawnSegment();
                RemoveOldSegment();
            }
        }

        /// <summary>
        /// Spawns the initial set of level segments.
        /// </summary>
        private void InitializeLevel()
        {
            for (int i = 0; i < initialSegments; i++)
            {
                SpawnSegment();
            }
        }

        /// <summary>
        /// Spawns a new, randomly selected level segment at the end of the level.
        /// </summary>
        private void SpawnSegment()
        {
            int randomIndex = Random.Range(0, levelSegments.Length);
            GameObject newSegment = Instantiate(levelSegments[randomIndex], transform.forward * spawnPosition, Quaternion.identity, transform);
            activeSegments.Add(newSegment);
            spawnPosition += segmentLength;
        }

        /// <summary>
        /// Removes the oldest level segment that is behind the player.
        /// </summary>
        private void RemoveOldSegment()
        {
            if (activeSegments.Count > 0)
            {
                GameObject oldSegment = activeSegments[0];
                activeSegments.RemoveAt(0);
                Destroy(oldSegment);
            }
        }

        /// <summary>
        /// Resets the level to its initial state by clearing all segments and re-initializing.
        /// </summary>
        private void ResetLevel()
        {
            // Destroy all currently active segments
            foreach (var segment in activeSegments)
            {
                Destroy(segment);
            }
            activeSegments.Clear();
            spawnPosition = 0;

            // Spawn the initial segments again
            InitializeLevel();
        }

        /// <summary>
        /// Responds to game state changes.
        /// </summary>
        /// <param name="newState">The new game state.</param>
        private void OnGameStateChanged(GameManager.GameState newState)
        {
            // Reset the level when returning to the main menu
            if (newState == GameManager.GameState.MainMenu)
            {
                ResetLevel();
            }
        }
    }
}
