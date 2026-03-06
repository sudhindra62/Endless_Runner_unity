using System.Collections.Generic;

// These enums should be defined in a globally accessible file.
public enum ObstacleType { Generic, Wall, Jump, Slide }
public enum DeathCause { ObstacleCollision, Fall, Boss }

/// <summary>
/// A data container for all player performance metrics tracked within a single run.
/// This object is managed by the AdaptiveDifficultyManager.
/// </summary>
public class RunSessionData
{
    // --- Configuration ---
    private const int HISTORY_CAPACITY = 30;

    // --- Analytical Data ---
    public Queue<bool> DodgeSuccessHistory { get; } = new Queue<bool>(HISTORY_CAPACITY);
    public Dictionary<ObstacleType, int> ObstacleHitCounts { get; } = new Dictionary<ObstacleType, int>();
    public Dictionary<DeathCause, int> DeathCauseCounts { get; } = new Dictionary<DeathCause, int>();
    public int MaxComboAchieved { get; set; }
    public int CombosBroken { get; private set; }

    public void RecordDodge(bool success)
    {
        if (DodgeSuccessHistory.Count >= HISTORY_CAPACITY) DodgeSuccessHistory.Dequeue();
        DodgeSuccessHistory.Enqueue(success);
    }

    public void RecordObstacleHit(ObstacleType type)
    {
        if (!ObstacleHitCounts.ContainsKey(type)) ObstacleHitCounts[type] = 0;
        ObstacleHitCounts[type]++;
    }

    public void RecordDeath(DeathCause cause)
    {
        if (!DeathCauseCounts.ContainsKey(cause)) DeathCauseCounts[cause] = 0;
        DeathCauseCounts[cause]++;
    }

    public void RecordComboBroken()
    {
        CombosBroken++;
    }

    public void Reset()
    {
        DodgeSuccessHistory.Clear();
        ObstacleHitCounts.Clear();
        DeathCauseCounts.Clear();
        MaxComboAchieved = 0;
        CombosBroken = 0;
    }
}
