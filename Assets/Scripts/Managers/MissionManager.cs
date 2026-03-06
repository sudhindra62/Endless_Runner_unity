using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MissionManager : Singleton<MissionManager>
{
    [SerializeField] private MissionDatabase missionDatabase;
    public List<Mission> ActiveMissions { get; private set; } = new List<Mission>();

    private void Start()
    {
        // Load missions from the database
        if (missionDatabase != null)
        {
            foreach (var mission in missionDatabase.missions)
            {
                AddMission(mission.MissionId);
            }
        }
    }

    public void AddMission(string missionId)
    {
        if (missionDatabase == null) return;
        Mission missionToAdd = missionDatabase.missions.FirstOrDefault(m => m.MissionId == missionId);
        if (missionToAdd != null && !ActiveMissions.Any(m => m.MissionId == missionId))
        {
            ActiveMissions.Add(new Mission(missionToAdd.MissionId, missionToAdd.Type, missionToAdd.Target));
        }
    }

    public void UpdateMissionProgress(MissionType type, float value)
    {
        foreach (var mission in ActiveMissions)
        {
            if (mission.Type == type)
            {
                mission.UpdateProgress(value);
            }
        }
    }

    public Mission GetClosestMission()
    {
        Mission closestMission = null;
        float smallestDifference = float.MaxValue;

        foreach (var mission in ActiveMissions)
        {
            if (!mission.IsCompleted)
            {
                float difference = mission.GetDifference();
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    closestMission = mission;
                }
            }
        }
        return closestMission;
    }
}
