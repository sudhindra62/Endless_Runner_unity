using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A single segment of the track, handled by the level generation system.
/// Global scope.
/// </summary>
public class TrackSegment : MonoBehaviour
{
    [Header("Segment Configuration")]
    public float segmentLength = 30f;
    public Transform anchorPoint;
    public Transform connectionPoint;

    [Header("Placeholders")]
    public List<Transform> obstacleSlots = new List<Transform>();
    public List<Transform> coinPaths = new List<Transform>();

    // Logic for objects/obstacles/coins would go here or in a separate generator.
}
