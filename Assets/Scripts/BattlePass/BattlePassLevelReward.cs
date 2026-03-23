
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
    public RewardItem freeTrackReward;
    public RewardItem premiumTrackReward;
}

[Serializable]
public class BattlePassProgressData
{
    public int currentXP;
    public int currentLevel;
    public bool hasPremiumPass;
}
