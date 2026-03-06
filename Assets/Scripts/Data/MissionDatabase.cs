
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDatabase", menuName = "Missions/Mission Database")]
public class MissionDatabase : ScriptableObject
{
    public System.Collections.Generic.List<MissionData> allMissions;
}
