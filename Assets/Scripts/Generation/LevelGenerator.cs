
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Generation.Patterns;

namespace EndlessRunner.Generation
{
    /// <summary>
    /// Manages the instantiation and destruction of level segments (patterns).
    /// It works in conjunction with the ProceduralPatternEngine to build the level.
    /// </summary>
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        [Header("Generator Settings")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float generationLookAhead = 50.0f; // How far ahead of the player to generate
        [SerializeField] private float destructionLookBehind = 30.0f; // How far behind the player to destroy

        private List<GameObject> activePatterns = new List<GameObject>();
        private Vector3 nextPatternPosition = Vector3.zero;
        private LevelPattern lastPattern;
        private bool isGenerating = false;

        public void StartGenerating()
        {
            isGenerating = true;
            GenerateInitialSegments();
        }

        public void StopGenerating()
        {
            isGenerating = false;
        }

        private void Update()
        {
            if (!isGenerating) return;

            // Continuously generate new segments as the player moves forward
            if (playerTransform.position.z + generationLookAhead > nextPatternPosition.z)
            {
                GenerateNextSegment();
            }

            // Clean up old segments that are behind the player
            CleanupOldSegments();
        }

        private void GenerateInitialSegments()
        {
            // Always start with the designated starting pattern
            lastPattern = ProceduralPatternEngine.Instance.GetStartingPattern();
            if (lastPattern != null)
            {
                InstantiatePattern(lastPattern);
            }
            else
            {
                Debug.LogError("LEVEL_GENERATOR: No valid starting pattern found!");
                return;
            }

            // Pre-warm the level with a few segments
            for (int i = 0; i < 5; i++)
            {
                GenerateNextSegment();
            }
        }

        private void GenerateNextSegment()
        {
            LevelPattern nextPattern = ProceduralPatternEngine.Instance.SelectNextPattern(lastPattern);
            if (nextPattern != null)
            {
                InstantiatePattern(nextPattern);
                lastPattern = nextPattern;
            }
            else
            {
                Debug.LogWarning("LEVEL_GENERATOR: Failed to select a next pattern.");
            }
        }

        private void InstantiatePattern(LevelPattern pattern)
        {
            GameObject patternInstance = Instantiate(pattern.patternPrefab, nextPatternPosition, Quaternion.identity, transform);
            activePatterns.Add(patternInstance);
            nextPatternPosition.z += pattern.patternLength; // Move the spawn point forward
        }

        private void CleanupOldSegments()
        {
            // Use a temporary list to avoid modifying the list while iterating
            List<GameObject> patternsToDestroy = new List<GameObject>();

            foreach (var pattern in activePatterns)
            {
                // Check if the pattern's end position is well behind the player
                if (pattern.transform.position.z + 50 < playerTransform.position.z - destructionLookBehind)
                {
                    patternsToDestroy.Add(pattern);
                }
            }

            foreach (var pattern in patternsToDestroy)
            {
                activePatterns.Remove(pattern);
                Destroy(pattern);
            }
        }

        /// <summary>
        /// Resets the generator to its initial state for a new game.
        /// </summary>
        public void ResetGenerator()
        {
            StopGenerating();

            foreach (var pattern in activePatterns)
            {
                Destroy(pattern);
            }

            activePatterns.Clear();
            nextPatternPosition = Vector3.zero;
            lastPattern = null;
        }
    }
}
