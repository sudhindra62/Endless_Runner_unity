
using UnityEngine;
using System.Collections.Generic;

public class ShardInventoryManager : MonoBehaviour
{
    public static ShardInventoryManager Instance { get; private set; }

    // ItemID -> Shards owned
    private Dictionary<string, int> shardInventory = new Dictionary<string, int>();

    // ItemID -> Shards required for unlock
    private Dictionary<string, int> shardUnlockRequirements = new Dictionary<string, int>()
    {
        { "LEGENDARY_SWORD_OF_DOOM", 100 },
        { "SKIN_VETERAN_ARMOR", 50 }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // To be called by SaveManager on game load
    public void LoadShardInventory(Dictionary<string, int> loadedInventory)
    {
        shardInventory = loadedInventory ?? new Dictionary<string, int>();
    }

    public void AddShards(string itemID, int amount)
    {
        if (!shardInventory.ContainsKey(itemID))
        {
            shardInventory[itemID] = 0;
        }

        shardInventory[itemID] += amount;
        Debug.Log($"Added {amount} shards for {itemID}. Total: {shardInventory[itemID]}.");

        CheckForUnlock(itemID);
        // SaveManager.Instance.SaveGame(); // Save after adding shards
    }

    private void CheckForUnlock(string itemID)
    {
        if (shardInventory.ContainsKey(itemID) && shardUnlockRequirements.ContainsKey(itemID))
        {
            if (shardInventory[itemID] >= shardUnlockRequirements[itemID])
            {
                // Consume shards for unlock
                shardInventory[itemID] -= shardUnlockRequirements[itemID];

                // Route unlock through the proper authority
                if (itemID.StartsWith("SKIN_"))
                {
                    SkinManager.Instance.UnlockSkin(itemID);
                }
                else if (itemID.StartsWith("EFFECT_"))
                {
                    CosmeticEffectManager.Instance.UnlockEffect(itemID);
                }
                // Add other types as needed (e.g., TrailManager)

                Debug.Log($"<color=green>Item {itemID} unlocked via shards!</color>");
            }
        }
    }

    public int GetShardCount(string itemID)
    {
        return shardInventory.ContainsKey(itemID) ? shardInventory[itemID] : 0;
    }
}
