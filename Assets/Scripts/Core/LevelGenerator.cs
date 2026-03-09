
using System.Collections.Generic;
using UnityEngine;

namespace Core
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

        protected override void Awake()
        {
            base.Awake();
            if (playerTransform == null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            _initialPosition = transform.position;
        }

        private void Start()
        {
            InitializeLevel();
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

            if (playerTransform.position.z + generationTriggerDistance > _nextChunkPosition.z)
            {
                GenerateAndPlaceChunk();
            }

            if (_activeChunks.Count > chunksToMaintain)
            {
                GameObject chunkToDeactivate = _activeChunks.Dequeue();
                chunkToDeactivate.SetActive(false);
            }
        }

        private void InitializeLevel()
        {
            _nextChunkPosition = _initialPosition;
            for (int i = 0; i < initialChunks; i++)
            {
                GenerateAndPlaceChunk();
            }
        }

        private void GenerateAndPlaceChunk()
        {
            int[] pattern = patternGenerator.GeneratePattern(chunkLength);
            GameObject newChunk = ObjectPooler.Instance.SpawnFromPool("LevelChunk", _nextChunkPosition, Quaternion.identity);
            _nextChunkPosition.z += chunkLength;

            LevelChunk chunkComponent = newChunk.GetComponent<LevelChunk>();
            if (chunkComponent != null)
            {
                chunkComponent.Initialize(pattern);
            }

            _activeChunks.Enqueue(newChunk);
        }

        public void Reset()
        {
            foreach (var chunk in _activeChunks)
            {
                chunk.SetActive(false);
            }
            _activeChunks.Clear();
            transform.position = _initialPosition;
            InitializeLevel();
        }
    }
}
