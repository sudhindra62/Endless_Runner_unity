
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MilestoneDatabase", menuName = "Gameplay/Milestone Database")]
public class MilestoneDatabase : ScriptableObject
{
    public List<Milestone> Milestones;

    public List<Milestone> GetMilestonesByType(string type)
    {
        return Milestones.FindAll(m => m.MilestoneType == type);
    }
}
