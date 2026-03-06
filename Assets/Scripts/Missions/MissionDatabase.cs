using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MissionDatabase", menuName = "Missions/Mission Database")]
public class MissionDatabase : ScriptableObject
{
    public List<Mission> missions;
}
