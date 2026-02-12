using System.Collections.Generic;

public partial class DailyMissionManager
{
    // Compatibility read-only alias
    public IReadOnlyList<MissionStatus> ActiveMissions => GetActiveMissions();
}
