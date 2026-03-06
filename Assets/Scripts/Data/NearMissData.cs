using UnityEngine;

/// <summary>
/// Contains contextual information about a processed Near-Miss event.
/// </summary>
public class NearMissData
{
    public Vector3 ObstaclePosition { get; }
    public float Proximity { get; } // How close the miss was, for scaling effects.

    public NearMissData(Vector3 obstaclePosition, float proximity)
    {
        ObstaclePosition = obstaclePosition;
        Proximity = proximity;
    }
}
