using UnityEngine;

/// <summary>
/// Aggregates run session data and provides a method for serialization.
/// </summary>
[CreateAssetMenu(fileName = "RunSessionData", menuName = "RPG/Run Session Data")]
public class RunSessionData : ScriptableObject
{
    // Data fields
    public float TotalScore { get; set; }
    public float TotalTime { get; set; }
    public int ObstaclesDodged { get; set; }
    public int PerfectDodges { get; set; }
    public int RevivesUsed { get; set; }

    /// <summary>
    /// Resets all data to default values.
    /// </summary>
    public void Reset()
    {
        TotalScore = 0;
        TotalTime = 0;
        ObstaclesDodged = 0;
        PerfectDodges = 0;
        RevivesUsed = 0;
    }

    /// <summary>
    /// Creates a plain data object for serialization.
    /// </summary>
    /// <returns>A plain data object with the current run data.</returns>
    public RunSessionData_Plain ToPlainData()
    {
        return new RunSessionData_Plain(this);
    }
}
