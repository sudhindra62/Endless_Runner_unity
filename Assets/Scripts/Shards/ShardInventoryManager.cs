
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the player's shard inventory and facilitates item unlocking through ShardableItemProfiles.
/// Refactored by the Supreme Guardian Architect v12 to be data-driven and modular.
/// </summary>
public class ShardInventoryManager : Singleton<ShardInventoryManager>
{
    // --- STATE ---
    private ShardInventoryData _inventoryData;

    // --- CONFIGURATION ---
    // In a real project, these would be loaded from an Addressables group or a Resources folder.
    private Dictionary<string, ShardableItemProfile> _shardableItemRegistry = new Dictionary<string, ShardableItemProfile>();

    protected override void Awake()
    {
        base.Awake();
        // You would populate the registry here, e.g., by loading all ShardableItemProfile assets.
        // For this example, we will assume it's populated.
    }

    public void RegisterShardableItem(ShardableItemProfile itemProfile)
    {
        if (itemProfile == null || string.IsNullOrEmpty(itemProfile.shardId))
            return;

        if (!_shardableItemRegistry.ContainsKey(itemProfile.shardId))
        {
            _shardableItemRegistry.Add(itemProfile.shardId, itemProfile);
        }
    }

    // To be called by a central SaveManager
    public void LoadInventoryData(ShardInventoryData data)
    {
        _inventoryData = data ?? new ShardInventoryData();
        Debug.Log("Guardian Architect: ShardInventoryManager data loaded.");
    }

    public ShardInventoryData GetInventoryDataForSaving()
    {
        return _inventoryData;
    }

    /// <summary>
    /// Adds shards to the player's inventory and immediately checks if an item can be unlocked.
    /// </summary>
    /// <param name="shardId">The unique ID of the shards being added.</param>
    /// <param name="amount">The number of shards to add.</param>
    public void AddShardsAndCheckForUnlock(string shardId, int amount)
    {
        if (_inventoryData == null || amount <= 0) return;

        _inventoryData.AddShards(shardId, amount);
        Debug.Log($"Guardian Architect: Added {amount} shards for '{shardId}'. Total: {_inventoryData.GetShardCount(shardId)}.");

        // Check if this addition can unlock an item
        if (_shardableItemRegistry.TryGetValue(shardId, out ShardableItemProfile itemProfile))
        {
            int currentShards = _inventoryData.GetShardCount(shardId);
            if (currentShards >= itemProfile.shardsRequired)
            {
                if (_inventoryData.ConsumeShards(shardId, itemProfile.shardsRequired))
                {
                    // Route the unlock logic through the profile itself
                    itemProfile.OnUnlock(null); // Pass player data if needed
                    Debug.Log($"<color=green>Guardian Architect: Item unlocked for shard '{shardId}'!</color>");
                }
            }
        }
    }

    /// <summary>
    /// Gets the current count of a specific shard type.
    /// </summary>
    public int GetShardCount(string shardId)
    {
        return _inventoryData?.GetShardCount(shardId) ?? 0;
    }
}
