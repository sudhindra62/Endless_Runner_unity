using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Level;

namespace EndlessRunner.LevelSystem
{
    public class SegmentPoolManager : MonoBehaviour
    {
        public static SegmentPoolManager Instance { get; private set; }

        private Dictionary<string, Queue<TrackSegment>> _segmentPool = new Dictionary<string, Queue<TrackSegment>>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void CreatePool(GameObject prefab, int initialSize)
        {
            if (!_segmentPool.ContainsKey(prefab.name))
            {
                _segmentPool[prefab.name] = new Queue<TrackSegment>();
                for (int i = 0; i < initialSize; i++)
                {
                    var segment = Instantiate(prefab).GetComponent<TrackSegment>();
                    segment.gameObject.SetActive(false);
                    _segmentPool[prefab.name].Enqueue(segment);
                }
            }
        }

        public TrackSegment GetSegment(GameObject prefab)
        {
            if (_segmentPool.ContainsKey(prefab.name) && _segmentPool[prefab.name].Count > 0)
            {
                var segment = _segmentPool[prefab.name].Dequeue();
                segment.gameObject.SetActive(true);
                return segment;
            }
            else
            {
                var segment = Instantiate(prefab).GetComponent<TrackSegment>();
                segment.prefab = prefab;
                return segment;
            }
        }

        public void ReturnSegment(TrackSegment segment)
        {
            segment.gameObject.SetActive(false);
             if (segment.prefab != null) // Check if the prefab info is attached
            {
                if (!_segmentPool.ContainsKey(segment.prefab.name))
                {
                    _segmentPool[segment.prefab.name] = new Queue<TrackSegment>();
                }
                _segmentPool[segment.prefab.name].Enqueue(segment);
            }
            else
            {
                Debug.LogWarning("Attempted to return a segment to the pool, but its prefab type is unknown. The segment will be destroyed instead.");
                Destroy(segment.gameObject);
            }
        }
    }
}
