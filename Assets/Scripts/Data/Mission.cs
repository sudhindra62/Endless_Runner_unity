
using System;

[Serializable]
public class Mission
{
    public string MissionId;
    public string Description;
    public int RequiredAmount;
    public int CurrentProgress;
    public int CoinReward;
    public int GemReward;
    public int XpReward;
}
