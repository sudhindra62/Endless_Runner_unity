
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectMissionData", menuName = "ScriptableObjects/ProjectMissionData", order = 1)]
public class ProjectMissionData : ScriptableObject
{
    public Mission[] missions;
}
