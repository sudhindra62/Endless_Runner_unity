
using System;

[Serializable]
public class BattlePassReward
{
    public string itemID;
    public int quantity;
}

[Serializable]
public class BattlePassLevelReward
{
    public int level;
    public BattlePassReward freeTrackReward;
    public BattlePassReward premiumTrackReward;
}
