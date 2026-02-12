using System;

/// <summary>
/// Holds runtime progress for a mission.
/// Links to MissionData without modifying it.
/// </summary>
[Serializable]
public class MissionStatus
{
    public MissionData Data;
    public int CurrentProgress;
    public bool IsClaimed;

    public MissionStatus(MissionData data)
    {
        Data = data;
        CurrentProgress = 0;
        IsClaimed = false;
    }
}
