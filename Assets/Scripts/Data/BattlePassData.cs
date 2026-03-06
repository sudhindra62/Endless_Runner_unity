
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct RewardItem
{
    public string itemID; // Could be a ScriptableObject ID or a simple string
    public int quantity;
    public Sprite icon;
    // public ItemType itemType; // e.g., Currency, Skin, PowerUp
}

[System.Serializable]
public struct BattlePassTier
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
    public List<BattlePassTier> tiers;
}
