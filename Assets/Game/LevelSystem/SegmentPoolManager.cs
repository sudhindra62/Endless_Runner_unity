using UnityEngine;
using System.Collections.Generic;

public class SegmentPoolManager : MonoBehaviour
{
    public static SegmentPoolManager Instance { get; private set; }

    public LevelGenerator levelGenerator;
    private Dictionary<int, Queue<TrackSegment>> segmentPools = new Dictionary<int, Queue<TrackSegment>>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreatePools();
    }

    void CreatePools()
    {
        for (int i = 0; i < levelGenerator.segmentPrefabs.Length; i++)
        {
            segmentPools.Add(i, new Queue<TrackSegment>());
        }
    }

    public TrackSegment GetSegment(int type)
    {
        if (segmentPools[type].Count > 0)
        {
            TrackSegment segment = segmentPools[type].Dequeue();
            segment.gameObject.SetActive(true);
            return segment;
        }
        else
        {
            TrackSegment newSegment = Instantiate(levelGenerator.segmentPrefabs[type]);
            newSegment.segmentType = type;
            return newSegment;
        }
    }

    public void ReturnSegment(TrackSegment segment)
    {
        segment.gameObject.SetActive(false);
        segmentPools[segment.segmentType].Enqueue(segment);
    }
}
