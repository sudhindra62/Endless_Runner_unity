
using UnityEngine;

/// <summary>
/// Manages the player's style points, rewarding stylish maneuvers with score bonuses.
/// </summary>
public class StyleManager : MonoBehaviour
{
    public static StyleManager Instance { get; private set; }
    public int StylePoints { get; private set; }
    public float BonusMultiplier { get; private set; } = 1f;
    public float currentStyle => StylePoints;
    public float maxStyle => 100f;
    public float CurrentMultiplier => BonusMultiplier;

    [Header("Style Point Configuration")]
    [SerializeField] private int pointsForNearMiss = 10;
    [SerializeField] private int pointsForPerfectLaneSwitch = 5;

    private void Start()
    {
        if (Instance == null) Instance = this;
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
        BonusMultiplier = 1f;
    }

    public void SetBonusMultiplier(float multiplier) => BonusMultiplier = multiplier;
    public float GetBonusMultiplier() => BonusMultiplier;
}
