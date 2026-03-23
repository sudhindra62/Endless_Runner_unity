
using System.Collections.Generic;

/// <summary>
/// Represents the data for a single game run.
/// </summary>
public class RunSessionData
{
    public int Score;
    public int Coins;
    public float Distance;
    public List<bool> DodgeSuccessHistory = new List<bool>();
    public Dictionary<ObstacleType, int> ObstacleHitCounts = new Dictionary<ObstacleType, int>();
    public DeathCause? DeathCause;

    public int score { get => Score; set => Score = value; }
    public float distance { get => Distance; set => Distance = value; }
    public int reviveCount;
    public int styleScore;
    public int comboPeak;
    public int riskLaneUsage;
    public float duration;

    // Extended fields for advanced analytics and Integrity checks
    public int TotalScore => Score;
    public float TotalTime;
    public int ObstaclesDodged => DodgeSuccessHistory.FindAll(b => b).Count;
    public int PerfectDodges;
    public int RevivesUsed => reviveCount;
    public bool hasUsedTimeWarp;
    public float highestMultiplier;

    public RunSessionData() : this(0, 0, 0f, new List<bool>(), new Dictionary<ObstacleType, int>(), null)
    {
    }

    public RunSessionData(int score, int coins, float distance, List<bool> dodgeSuccessHistory, Dictionary<ObstacleType, int> obstacleHitCounts, DeathCause? deathCause)
    {
        Score = score;
        Coins = coins;
        Distance = distance;
        DodgeSuccessHistory = dodgeSuccessHistory;
        ObstacleHitCounts = obstacleHitCounts;
        DeathCause = deathCause;
    }

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
        Score = 0;
        Coins = 0;
        Distance = 0;
        DodgeSuccessHistory.Clear();
        ObstacleHitCounts.Clear();
        DeathCause = null;
    }
}
