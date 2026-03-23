using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A centralized database of all available missions in the game.
/// Global scope.
/// </summary>
[CreateAssetMenu(fileName = "MissionDatabase", menuName = "Endless Runner/Data/Mission Database")]
public class MissionDatabase : ScriptableObject
{
    public List<Mission> missions;

    public Mission GetMissionByID(string id)
    {
        return missions.Find(m => m.missionId == id);
    }
}
