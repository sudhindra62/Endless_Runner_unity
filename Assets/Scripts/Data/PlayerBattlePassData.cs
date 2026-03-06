
using System;

[Serializable]
public class PlayerBattlePassData
{
    public int currentXP;
    public int currentLevel;
    public bool hasPremiumPass;
    public DateTime seasonStartDate;

    public PlayerBattlePassData()
    {
        currentXP = 0;
        currentLevel = 1;
        hasPremiumPass = false;
        seasonStartDate = DateTime.UtcNow;
    }
}
