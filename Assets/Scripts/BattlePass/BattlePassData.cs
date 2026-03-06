
using System;

[Serializable]
public class BattlePassData
{
    public int currentXP;
    public int currentLevel;
    public bool hasPremiumPass;
    public DateTime seasonStartDate;

    public BattlePassData()
    {
        currentXP = 0;
        currentLevel = 1;
        hasPremiumPass = false;
        seasonStartDate = DateTime.UtcNow;
    }
}
