using UnityEngine;
using System.Collections.Generic;

public class SegmentPoolManager : MonoBehaviour
{
    public static SegmentPoolManager Instance { get; private set; }

    private Dictionary<GameObject, List<TrackSegment>> segmentPools = new Dictionary<GameObject, List<TrackSegment>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public TrackSegment GetSegment(GameObject segmentPrefab)
    {
        if (!segmentPools.ContainsKey(segmentPrefab) || segmentPools[segmentPrefab].Count == 0)
        {
            // Instantiate a new segment if the pool is empty
            return Instantiate(segmentPrefab).GetComponent<TrackSegment>();
        }

        // Get a segment from the pool
        TrackSegment segment = segmentPools[segmentPrefab][0];
        segmentPools[segmentPrefab].RemoveAt(0);
        segment.gameObject.SetActive(true);
        return segment;
    }

    public void ReturnSegment(TrackSegment segment)
    {
        segment.gameObject.SetActive(false);
        GameObject prefab = segment.gameObject;

        if (!segmentPools.ContainsKey(prefab))
        {
            segmentPools[prefab] = new List<TrackSegment>();
        }

        segmentPools[prefab].Add(segment);
    }
}
