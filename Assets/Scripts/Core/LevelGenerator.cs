
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.Generation
{
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        [Header("Level Geometry")]
        [SerializeField] private GameObject levelChunkPrefab;
        [SerializeField] private int chunkLength = 30;
        [SerializeField] private int initialChunks = 5;
        [SerializeField] private int chunksToMaintain = 5;

        [Header("Generation Control")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float generationTriggerDistance = 15f;

        [Header("Pattern Generation")]
        [SerializeField] private ProceduralPatternGenerator patternGenerator;

        private Queue<GameObject> _activeChunks = new Queue<GameObject>();
        private Vector3 _nextChunkPosition;
        private Vector3 _initialPosition;
        private bool _isGenerating = false;

        protected override void Awake()
        {
            base.Awake();
            if (playerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }
            _initialPosition = transform.position;
        }

        private void Update()
        {
            if (!_isGenerating || playerTransform == null) return;

            // Check if the player has advanced far enough to trigger generating the next chunk.
            if (playerTransform.position.z + generationTriggerDistance > _nextChunkPosition.z)
            {
                GenerateAndPlaceChunk();
            }

            // Maintain the desired number of active chunks, recycling the oldest ones.
            while (_activeChunks.Count > chunksToMaintain)
            {
                GameObject chunkToRecycle = _activeChunks.Dequeue();
                chunkToRecycle.SetActive(false); // Return to pool instead of destroying
            }
        }

        public void StartGenerating()
        {
            if (_isGenerating) return;

            ResetGeneratorState();
            InitializeLevel();
            _isGenerating = true;
            this.enabled = true; // Ensure the Update loop runs.
        }

        public void StopGenerating()
        {
            _isGenerating = false;
            this.enabled = false; // Stop the Update loop to save performance.
        }

        private void InitializeLevel()
        {
            for (int i = 0; i < initialChunks; i++)
            {
                GenerateAndPlaceChunk();
            }
        }

        private void GenerateAndPlaceChunk()
        {
            // Use an Object Pooler to get a recycled Level Chunk
            GameObject newChunk = ObjectPooler.Instance.SpawnFromPool("LevelChunk", _nextChunkPosition, Quaternion.identity);
            _nextChunkPosition.z += chunkLength;

            // TODO: In a full implementation, you would generate a pattern for obstacles here.
            // For now, we just place the chunk.

            _activeChunks.Enqueue(newChunk);
        }

        private void ResetGeneratorState()
        {
            // Return all active chunks to the pool
            foreach (var chunk in _activeChunks)
            {
                chunk.SetActive(false);
            }
            _activeChunks.Clear();

            // Reset the generator's position to its starting point.
            transform.position = _initialPosition;
            _nextChunkPosition = _initialPosition;
        }
    }
}
