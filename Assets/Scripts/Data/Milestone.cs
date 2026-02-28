using UnityEngine;

/// <summary>
/// Defines a long-term milestone, including its goal and reward.
/// </summary>
[CreateAssetMenu(fileName = "New Milestone", menuName = "RPG/Milestone")]
public class Milestone : ScriptableObject
{
    public string milestoneName;
    public string description;
    public int goal;
    public int reward;
}
