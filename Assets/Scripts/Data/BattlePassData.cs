
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct BattlePassTierData
{
    public int xpRequired;
    public List<RewardItem> freeRewards;
    public List<RewardItem> premiumRewards;
}

[CreateAssetMenu(fileName = "NewBattlePass", menuName = "Gameplay/Battle Pass/New Battle Pass")]
public class BattlePassData : ScriptableObject
{
    public string passName;
    public int seasonNumber;
    public List<BattlePassTierData> tiers;
}
