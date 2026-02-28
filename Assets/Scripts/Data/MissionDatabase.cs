
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDatabase", menuName = "Gameplay/Mission Database")]
public class MissionDatabase : ScriptableObject
{
    public List<Mission> Missions;

    public Mission GetMissionById(string missionId)
    {
        return Missions.Find(m => m.MissionId == missionId);
    }
}
