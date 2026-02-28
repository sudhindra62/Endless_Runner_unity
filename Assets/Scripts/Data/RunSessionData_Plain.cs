using System.Collections.Generic;

/// <summary>
/// A plain C# class for storing run session data. This is useful for serialization.
/// </summary>
[System.Serializable]
public class RunSessionData_Plain
{
    public float totalScore;
    public float totalTime;
    public int obstaclesDodged;
    public int perfectDodges;
    public int revivesUsed;

    public RunSessionData_Plain(RunSessionData data)
    {
        totalScore = data.TotalScore;
        totalTime = data.TotalTime;
        obstaclesDodged = data.ObstaclesDodged;
        perfectDodges = data.PerfectDodges;
        revivesUsed = data.RevivesUsed;
    }
}
