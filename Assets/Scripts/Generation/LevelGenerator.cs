
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Player;
using EndlessRunner.Generation.Patterns;

namespace EndlessRunner.Generation
{
    /// <summary>
    /// Manages the instantiation and destruction of level segments (patterns).
    /// </summary>
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        [Header("Generator Settings")]
        [SerializeField] private float generationLookAhead = 50.0f;
        [SerializeField] private float destructionLookBehind = 30.0f;

        private Transform playerTransform;
        private List<GameObject> activePatterns = new List<GameObject>();
        private Vector3 nextPatternPosition = Vector3.zero;
        private LevelPattern lastPattern;
        private bool isGenerating = false;

        private void Start()
        {
            playerTransform = PlayerController.Instance.transform;
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        private void Update()
        {
            if (!isGenerating) return;

            if (playerTransform.position.z + generationLookAhead > nextPatternPosition.z)
            {
                GenerateNextSegment();
            }

            CleanupOldSegments();
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.Playing)
            {
                StartGenerating();
            }
            else
            {
                StopGenerating();
            }
        }

        private void StartGenerating()
        {
            if(isGenerating) return;

            isGenerating = true;
            if (activePatterns.Count == 0)
            {
                ResetGenerator();
                GenerateInitialSegments();
            }
        }

        private void StopGenerating()
        {
            isGenerating = false;
        }

        private void GenerateInitialSegments()
        {
            lastPattern = ProceduralPatternEngine.Instance.GetStartingPattern();
            if (lastPattern != null)
            {
                InstantiatePattern(lastPattern);
            }
            else
            {
                Debug.LogError("No valid starting pattern found!");
                return;
            }

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
                Debug.LogWarning("Failed to select a next pattern.");
            }
        }

        private void InstantiatePattern(LevelPattern pattern)
        {
            GameObject patternInstance = Instantiate(pattern.patternPrefab, nextPatternPosition, Quaternion.identity, transform);
            activePatterns.Add(patternInstance);
            nextPatternPosition.z += pattern.patternLength;
        }

        private void CleanupOldSegments()
        {
            List<GameObject> patternsToDestroy = new List<GameObject>();
            foreach (var pattern in activePatterns)
            {
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
