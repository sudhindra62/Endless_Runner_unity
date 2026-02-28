
using UnityEngine;

/// <summary>
/// Manages the player's style points, rewarding stylish maneuvers with score bonuses.
/// </summary>
public class StyleManager : MonoBehaviour
{
    public int StylePoints { get; private set; }

    [Header("Style Point Configuration")]
    [SerializeField] private int pointsForNearMiss = 10;
    [SerializeField] private int pointsForPerfectLaneSwitch = 5;

    private void Start()
    {
        // Reset style points at the start of a run
        ResetStylePoints();
    }

    /// <summary>
    /// Adds style points for a near miss with an obstacle.
    /// </summary>
    public void AddNearMissPoints()
    {
        StylePoints += pointsForNearMiss;
        Debug.Log($"Near miss! +{pointsForNearMiss} style points.");
    }

    /// <summary>
    /// Adds style points for a perfectly timed lane switch.
    /// </summary>
    public void AddPerfectLaneSwitchPoints()
    {
        StylePoints += pointsForPerfectLaneSwitch;
        Debug.Log($"Perfect lane switch! +{pointsForPerfectLaneSwitch} style points.");
    }

    /// <summary>
    /// Resets the style points to zero. Typically called at the beginning of a run.
    /// </summary>
    public void ResetStylePoints()
    {
        StylePoints = 0;
    }
}
