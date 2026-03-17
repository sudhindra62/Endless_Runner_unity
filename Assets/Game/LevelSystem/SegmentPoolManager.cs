using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Level
{
    public class SegmentPoolManager : MonoBehaviour
    {
        public static SegmentPoolManager Instance { get; private set; }

        public GameObject[] segmentPrefabs;
        public int poolSize = 10;

        private Dictionary<GameObject, Queue<GameObject>> segmentPools;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            segmentPools = new Dictionary<GameObject, Queue<GameObject>>();

            foreach (var prefab in segmentPrefabs)
            {
                var pool = new Queue<GameObject>();
                for (int i = 0; i < poolSize; i++)
                {
                    var segment = Instantiate(prefab, transform);
                    segment.SetActive(false);
                    pool.Enqueue(segment);
                }
                segmentPools.Add(prefab, pool);
            }
        }

        public GameObject GetSegment(GameObject prefab)
        {
            if (segmentPools.ContainsKey(prefab))
            {
                var pool = segmentPools[prefab];
                if (pool.Count > 0)
                {
                    var segment = pool.Dequeue();
                    segment.SetActive(true);
                    return segment;
                }
                else
                {
                    var segment = Instantiate(prefab);
                    segment.SetActive(true);
                    return segment;
                }
            }
            return null;
        }

        public void ReturnSegment(GameObject segment, GameObject prefab)
        {
            if (segmentPools.ContainsKey(prefab))
            {
                var pool = segmentPools[prefab];
                segment.SetActive(false);
                pool.Enqueue(segment);
            }
        }
    }
}
