
using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewChestData", menuName = "Endless Runner/Chest Data")]
public class ChestData : ScriptableObject
{
    [Serializable]
    public struct RewardDrop
    {
        // Can be currency name, item ID, etc.
        public string rewardId;
        public int minAmount;
        public int maxAmount;
        [Range(0f, 1f)] public float dropChance; // 0 to 1 chance
    }

    public string chestId;
    public float cooldownHours;

    public string chestName;
    public Sprite chestIcon;
    public List<RewardDrop> rewardPool = new List<RewardDrop>();

    /// <summary>
    /// Calculates and returns a list of rewards based on the drop chances.
    /// </summary>
    /// <returns>A list of rewards to be granted.</returns>
    public List<KeyValuePair<string, int>> GetRewards()
    {
        var grantedRewards = new List<KeyValuePair<string, int>>();
        foreach (var drop in rewardPool)
        {
            if (UnityEngine.Random.value <= drop.dropChance)
            {
                int amount = UnityEngine.Random.Range(drop.minAmount, drop.maxAmount + 1);
                grantedRewards.Add(new KeyValuePair<string, int>(drop.rewardId, amount));
            }
        }
        return grantedRewards;
    }
}
