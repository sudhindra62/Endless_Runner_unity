using UnityEngine;
using System.Collections.Generic;

namespace EndlessRunner.Level
{
    public class SpawnController : MonoBehaviour
    {
        public PlayerController player;
        public float spawnDistance = 50f;
        public float recycleDistance = 50f;

        private List<GameObject> activeSegments = new List<GameObject>();
        private Transform lastSpawnPoint;

        private void Start()
        {
            lastSpawnPoint = transform;
        }

        private void Update()
        {
            // Spawn new segments
            if (Vector3.Distance(player.transform.position, lastSpawnPoint.position) < spawnDistance)
            {
                var segmentObject = LevelGenerator.Instance.GetThemedSegment();
                if (segmentObject != null)
                {
                    var segment = segmentObject.GetComponent<TrackSegment>();
                    if (segment != null)
                    {
                        segment.transform.position = lastSpawnPoint.position;
                        segment.transform.rotation = lastSpawnPoint.rotation;
                        lastSpawnPoint = segment.GetNextSpawnPoint();
                        activeSegments.Add(segmentObject);

                        // Spawn obstacles and coins
                        if (ThemeManager.Instance != null && ThemeManager.Instance.currentTheme != null)
                        {
                            segment.SpawnObstaclesAndCoins(ThemeManager.Instance.currentTheme);
                        }
                    }
                }
            }

            // Recycle old segments
            for (int i = activeSegments.Count - 1; i >= 0; i--)
            {
                var segmentObject = activeSegments[i];
                if (Vector3.Distance(player.transform.position, segmentObject.transform.position) > recycleDistance)
                {
                    var segment = segmentObject.GetComponent<TrackSegment>();
                    if (segment != null)
                    {
                        SegmentPoolManager.Instance.ReturnSegment(segmentObject, segment.prefab);
                        activeSegments.RemoveAt(i);
                    }
                }
            }
        }
    }
}
