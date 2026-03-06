
using System.Collections.Generic;

/// <summary>
/// Represents the data for a single game run.
/// </summary>
public class RunSessionData
{
    public List<bool> DodgeSuccessHistory = new List<bool>();
    public Dictionary<ObstacleType, int> ObstacleHitCounts = new Dictionary<ObstacleType, int>();
    public DeathCause? DeathCause;

    public void RecordDodge(bool success)
    {
        DodgeSuccessHistory.Add(success);
    }

    public void RecordObstacleHit(ObstacleType type)
    {
        if (!ObstacleHitCounts.ContainsKey(type))
        {
            ObstacleHitCounts[type] = 0;
        }
        ObstacleHitCounts[type]++;
    }

    public void RecordDeath(DeathCause cause)
    {
        DeathCause = cause;
    }

    public void Reset()
    {
        DodgeSuccessHistory.Clear();
        ObstacleHitCounts.Clear();
        DeathCause = null;
    }
}
