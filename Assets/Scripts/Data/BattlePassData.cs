
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBattlePassSeason", menuName = "Endless Runner/Battle Pass Season")]
public class BattlePassData : ScriptableObject
{
    [System.Serializable]
    public struct RewardTier
    {
        public int xpRequired;
        public Reward freeTrackReward;
        public Reward premiumTrackReward;
    }

    [System.Serializable]
    public struct Reward
    {
        public string rewardId; // e.g., "Coins", "Gems", "Skin_Cyberpunk"
        public int amount;
        public Sprite icon;
    }

    public string seasonName;
    public int seasonNumber;
    public Sprite seasonIcon;
    public List<RewardTier> tiers = new List<RewardTier>();
}
