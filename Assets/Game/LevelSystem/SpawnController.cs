using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Level;

namespace EndlessRunner.LevelSystem
{
    public class SpawnController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float _spawnDistance = 50f;
        [SerializeField] private float _despawnDistance = 100f;
        [SerializeField] private int _initialPoolSize = 5;

        [Header("Segment Prefabs")]
        [SerializeField] private List<GameObject> _initialSegments;
        [SerializeField] private List<GameObject> _segmentPrefabs;

        private Transform _playerTransform;
        private Transform _nextSpawnPoint;
        private List<TrackSegment> _activeSegments = new List<TrackSegment>();

        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
            InitializeSegmentPools();
            InitializeLevel();
        }

        private void Update()
        {
            ManageSegmentSpawning();
            ManageSegmentDespawning();
        }

        private void InitializeSegmentPools()
        {
            foreach (var prefab in _segmentPrefabs)
            {
                SegmentPoolManager.Instance.CreatePool(prefab, _initialPoolSize);
            }
        }

        private void InitializeLevel()
        {
            _nextSpawnPoint = transform; // Start at the controller's position
            foreach (var segmentPrefab in _initialSegments)
            {
                SpawnSegment(segmentPrefab, false); // Don't spawn obstacles/coins on initial segments
            }
        }

        private void ManageSegmentSpawning()
        {
            if (_playerTransform == null) return;

            if (Vector3.Distance(_playerTransform.position, _nextSpawnPoint.position) < _spawnDistance)
            {
                SpawnRandomSegment();
            }
        }

        private void ManageSegmentDespawning()
        {
            for (int i = _activeSegments.Count - 1; i >= 0; i--)
            {
                TrackSegment segment = _activeSegments[i];
                if (_playerTransform.position.z - segment.endPoint.position.z > _despawnDistance)
                {
                    SegmentPoolManager.Instance.ReturnSegment(segment);
                    _activeSegments.RemoveAt(i);
                }
            }
        }

        private void SpawnRandomSegment()
        {
            int randIndex = Random.Range(0, _segmentPrefabs.Count);
            GameObject prefab = _segmentPrefabs[randIndex];
            SpawnSegment(prefab, true);
        }

        private void SpawnSegment(GameObject prefab, bool spawnContent)
        {
            TrackSegment segment = SegmentPoolManager.Instance.GetSegment(prefab);
            segment.transform.position = _nextSpawnPoint.position - segment.startPoint.localPosition;
            segment.transform.rotation = _nextSpawnPoint.rotation;

            _nextSpawnPoint = segment.GetNextSpawnPoint();
            _activeSegments.Add(segment);

            if (spawnContent)
            {
                // Get theme from LevelGenerator/ThemeManager
                // This assumes the LevelGenerator holds a reference to the ThemeManager
                var currentTheme = LevelGenerator.Instance.GetCurrentTheme();
                if (currentTheme != null)
                {
                    segment.SpawnObstaclesAndCoins(currentTheme);
                }
            }
        }
    }
}
