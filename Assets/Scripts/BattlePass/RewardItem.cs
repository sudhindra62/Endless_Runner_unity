using UnityEngine;

/// <summary>
/// A simple data structure representing a reward item.
/// Created by Supreme Guardian Architect v12 to establish a robust, data-driven reward system.
/// </summary>
[System.Serializable]
public struct RewardItem
{
    [Tooltip("The icon to display for this reward.")]
    public Sprite icon;

    [Tooltip("The quantity of the reward.")]
    public int quantity;

    [Tooltip("A description of the reward.")]
    public string description;
}
