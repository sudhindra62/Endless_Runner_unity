
using System;
using UnityEngine;

/// <summary>
/// Defines the type of reward a player can receive for leveling up.
/// </summary>
[Serializable]
public enum LevelRewardType
{
    Coins,
    Gems,
    Chest,
    SkinUnlock
}

/// <summary>
/// A serializable data container that defines a specific reward granted at a certain level.
/// This is a data-only class used by the LevelRewardManager.
/// </summary>
[Serializable]
public class LevelRewardData
{
    [Tooltip("The level at which this reward is granted.")]
    public int Level;

    [Tooltip("The type of reward.")]
    public LevelRewardType RewardType;

    [Tooltip("The amount of Coins/Gems, or the ChestType enum value.")]
    public int Amount;

    [Tooltip("The unique ID of the skin to unlock. Only used if RewardType is SkinUnlock.")]
    public string OptionalSkinID;
}
