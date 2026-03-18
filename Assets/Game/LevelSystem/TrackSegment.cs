using UnityEngine;
using System.Collections.Generic;

public class TrackSegment : MonoBehaviour
{
    [Tooltip("The type of the segment, used by the pool manager.")]
    public int segmentType;

    [Tooltip("The point where the next segment should be connected.")]
    public Transform connectionPoint;

    [Tooltip("A list of transforms representing locations where obstacles can be spawned.")]
    public List<Transform> obstacleSlots;

    [Tooltip("A list of transforms representing paths for coins to be spawned along.")]
    public List<Transform> coinPaths;
}
