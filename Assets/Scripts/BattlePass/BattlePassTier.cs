using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A ScriptableObject that defines the rewards for a single tier of the Battle Pass.
/// This allows for easy creation and management of Battle Pass tiers as assets in the project.
/// Created by Supreme Guardian Architect v12 to establish a robust, data-driven Battle Pass system.
/// </summary>
[CreateAssetMenu(fileName = "NewBattlePassTier", menuName = "Endless Runner/Battle Pass Tier")]
public class BattlePassTier : ScriptableObject
{
    [Header("Tier Information")]
    [Tooltip("The display name of the tier.")]
    public string tierName;

    [Header("Rewards")]
    [Tooltip("The list of rewards for the free track.")]
    public List<RewardItem> freeRewards;

    [Tooltip("The list of rewards for the premium track.")]
    public List<RewardItem> premiumRewards;

    // --- Single-item compatibility aliases for BattlePassManager.GetSeasonRewards() ---
    public RewardItem freeReward    => (freeRewards    != null && freeRewards.Count    > 0) ? freeRewards[0]    : default;
    public RewardItem premiumReward => (premiumRewards != null && premiumRewards.Count > 0) ? premiumRewards[0] : default;

    public bool HasFreeReward(int index = 0) => freeRewards != null && index < freeRewards.Count;
    public bool HasPremiumReward(int index = 0) => premiumRewards != null && index < premiumRewards.Count;
}
