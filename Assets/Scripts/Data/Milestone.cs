
using System;

[Serializable]
public class Milestone
{
    public string MilestoneId;
    public string Description;
    public string MilestoneType; // e.g., "Level", "Score", "Distance"
    public int RequiredAmount;
    public int CoinReward;
    public int GemReward;
    public int XpReward;
}
