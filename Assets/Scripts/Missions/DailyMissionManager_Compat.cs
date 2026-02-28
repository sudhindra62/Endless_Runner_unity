using System.Collections.Generic;

/// <summary>
/// Compatibility layer for the DailyMissionManager.
/// </summary>
public class DailyMissionManager_Compat
{
    private DailyMissionManager _dailyMissionManager;

    public DailyMissionManager_Compat(DailyMissionManager dailyMissionManager)
    {
        _dailyMissionManager = dailyMissionManager;
    }

    public List<MissionData_Compat> GetAdaptedMissions()
    {
        // This method would contain logic to adapt the DailyMissionManager's missions
        // to the new ProjectMissionData format. For now, it's a placeholder.
        return new List<MissionData_Compat>();
    }
}
