
using System.Collections.Generic;

/// <summary>
/// A data-centric class that encapsulates the player's shard inventory.
/// Authored by the Supreme Guardian Architect v12.
/// </summary>
[System.Serializable]
public class ShardInventoryData
{
    // A dictionary where the key is the shard's unique identifier and the value is the count.
    public Dictionary<string, int> ShardCounts { get; private set; } = new Dictionary<string, int>();

    /// <summary>
    /// Adds a specific amount of shards to the inventory.
    /// </summary>
    public void AddShards(string shardId, int amount)
    {
        if (!ShardCounts.ContainsKey(shardId))
        {
            ShardCounts[shardId] = 0;
        }
        ShardCounts[shardId] += amount;
    }

    /// <summary>
    /// Removes a specific amount of shards, typically after a craft or unlock.
    /// </summary>
    public bool ConsumeShards(string shardId, int amount)
    {
        if (ShardCounts.ContainsKey(shardId) && ShardCounts[shardId] >= amount)
        {
            ShardCounts[shardId] -= amount;
            if (ShardCounts[shardId] == 0)
            {
                ShardCounts.Remove(shardId);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the current count of a specific shard.
    /// </summary>
    public int GetShardCount(string shardId)
    {
        ShardCounts.TryGetValue(shardId, out int count);
        return count;
    }
}
