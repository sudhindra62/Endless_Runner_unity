using UnityEngine;
using System.Collections.Generic;

namespace EndlessRunner.Level
{
    public class SegmentPoolManager : MonoBehaviour
    {
        public static SegmentPoolManager Instance { get; private set; }

        private Dictionary<GameObject, Queue<GameObject>> _pool = new Dictionary<GameObject, Queue<GameObject>>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public GameObject GetSegment(GameObject prefab)
        {
            if (!_pool.ContainsKey(prefab) || _pool[prefab].Count == 0)
            {
                return Instantiate(prefab);
            }

            GameObject segment = _pool[prefab].Dequeue();
            segment.SetActive(true);
            return segment;
        }

        public void ReturnSegment(GameObject segment)
        {
            segment.SetActive(false);
            if (!_pool.ContainsKey(segment.GetComponent<TrackSegment>().prefab))
            {
                _pool[segment.GetComponent<TrackSegment>().prefab] = new Queue<GameObject>();
            }
            _pool[segment.GetComponent<TrackSegment>().prefab].Enqueue(segment);
        }
    }
}