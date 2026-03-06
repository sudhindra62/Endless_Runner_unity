
using System;

public enum MissionType
{
    Score,
    Distance,
    Coins,
    NearMiss,
    FeverTime
}

[Serializable]
public class Mission
{
    public string MissionId;
    public MissionType Type;
    public float Target;
    public float CurrentProgress;
    public bool IsCompleted;

    public Mission(string missionId, MissionType type, float target)
    {
        MissionId = missionId;
        Type = type;
        Target = target;
        CurrentProgress = 0;
        IsCompleted = false;
    }

    public void UpdateProgress(float value)
    {
        if (IsCompleted) return;
        CurrentProgress += value;
        if (CurrentProgress >= Target)
        {
            CurrentProgress = Target;
            IsCompleted = true;
        }
    }

    public float GetProgressPercentage()
    {
        return (CurrentProgress / Target) * 100f;
    }

    public float GetDifference()
    {
        return Target - CurrentProgress;
    }
}
