using UnityEngine;
using System.Collections.Generic;

public class TrackSegment : MonoBehaviour
{
    [Header("Segment Configuration")]
    public Transform connectionPoint;

    [Header("Placeholders")]
    public List<Transform> obstacleSlots = new List<Transform>();
    public List<Transform> coinPaths = new List<Transform>();
}
