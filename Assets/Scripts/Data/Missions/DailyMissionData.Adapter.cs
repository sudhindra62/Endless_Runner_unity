public partial class DailyMissionData
{
    public class Adapter
    {
        private DailyMissionData _dailyMissionData;

        public Adapter(DailyMissionData dailyMissionData)
        {
            _dailyMissionData = dailyMissionData;
        }

        public ProjectMissionData GetProjectMissionData()
        {
            return _dailyMissionData.missionData;
        }
    }
}
