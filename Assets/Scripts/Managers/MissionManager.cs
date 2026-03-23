using UnityEngine;

/// <summary>
/// Architectural proxy for Mission management.
/// Routes calls to the authoritative DailyMissionManager.
/// </summary>
public class MissionManager : Singleton<MissionManager>
{
    public void RefreshMissions()
    {
        if (DailyMissionManager.Instance != null) DailyMissionManager.Instance.RefreshMissions();
    }

    public bool AreMissionsReady()
    {
        return DailyMissionManager.Instance != null && DailyMissionManager.Instance.AreMissionsReady();
    }
}
