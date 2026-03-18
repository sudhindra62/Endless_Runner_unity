using UnityEngine;
using System.Collections.Generic;

namespace EndlessRunner.Level
{
    public class SpawnController : MonoBehaviour
    {
        [Header("Level Generation")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private TrackSegment[] segmentPrefabs;
        [SerializeField] private int initialSegments = 5;
        [SerializeField] private float spawnDistance = 50f;
        [SerializeField] private float despawnDistance = 50f;

        private List<GameObject> _activeSegments = new List<GameObject>();
        private Transform _currentSpawnPoint;
        private float _timeAlive = 0f;

        private void Start()
        {
            _currentSpawnPoint = transform;
            for (int i = 0; i < initialSegments; i++)
            {
                SpawnSegment();
            }
        }

        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (Vector3.Distance(playerTransform.position, _currentSpawnPoint.position) < spawnDistance)
            {
                SpawnSegment();
            }

            if (_activeSegments.Count > 0 && Vector3.Distance(playerTransform.position, _activeSegments[0].transform.position) > despawnDistance)
            {
                DespawnSegment();
            }
        }

        private void SpawnSegment()
        {
            ThemeConfig currentTheme = LevelGenerator.Instance.GetCurrentTheme();
            TrackSegment selectedSegmentPrefab = SelectSegmentBasedOnDifficulty();

            GameObject segmentObject = SegmentPoolManager.Instance.GetSegment(selectedSegmentPrefab.prefab);
            TrackSegment trackSegment = segmentObject.GetComponent<TrackSegment>();

            segmentObject.transform.position = _currentSpawnPoint.position;
            segmentObject.transform.rotation = _currentSpawnPoint.rotation;

            trackSegment.SpawnObstaclesAndCoins(currentTheme);

            _currentSpawnPoint = trackSegment.GetNextSpawnPoint();
            _activeSegments.Add(segmentObject);
        }

        private void DespawnSegment()
        {
            GameObject segmentToDespawn = _activeSegments[0];
            _activeSegments.RemoveAt(0);
            SegmentPoolManager.Instance.ReturnSegment(segmentToDespawn);
        }

        private TrackSegment SelectSegmentBasedOnDifficulty()
        {
            // Simple difficulty scaling: introduce more complex segments over time.
            float difficulty = Mathf.Clamp01(_timeAlive / 120f); // Full difficulty at 2 minutes
            int maxSegmentIndex = (int)(segmentPrefabs.Length * difficulty);
            int randomIndex = Random.Range(0, Mathf.Max(1, maxSegmentIndex));

            return segmentPrefabs[randomIndex];
        }
    }
}
