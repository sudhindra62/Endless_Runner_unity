
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

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

    /// <summary>
    /// This is the master function to call at the end of a run to process all rewards.
    /// </summary>
    public void ProcessEndOfRunRewards(RunSessionData runData, bool bossDefeated)
    {
        // Step 1: Calculate and award standard rewards (currency, XP, etc.)
        // (Existing reward calculation logic would go here)

        // Step 2: Trigger the rare drop evaluation AFTER standard rewards are calculated.
        // This fulfills the requirement: "After reward calculation. Before reward display."
        if (RareDropManager.Instance != null)
        {
            RareDropManager.Instance.EvaluateRareDrop(runData, bossDefeated);
        }

        // Step 3: Display all rewards to the player in the UI
        // (This would trigger the UI animation sequences for all collected rewards)
    }

    public void Award(string itemID, int quantity)
    {
        // This method is the designated authority for granting items.
        // It is called by RareDropManager and other systems.
        Debug.Log($"REWARD_MANAGER: Awarding {quantity} of {itemID}");

        if (itemID.StartsWith("SHARD_"))
        {
            // Note: RareDropManager now calls ShardInventoryManager directly for shard awards.
            // This route can be maintained for other shard sources (e.g., chests).
            ShardInventoryManager.Instance.AddShards(itemID, quantity); // Ensure correct method name
        }
        else if (itemID.StartsWith("SKIN_"))
        {
            SkinManager.Instance.UnlockSkin(itemID);
        }
        else if (itemID.StartsWith("EFFECT_"))
        {
            CosmeticEffectManager.Instance.UnlockEffect(itemID);
        }
        else
        {
            Debug.LogWarning($"No manager found to handle reward: {itemID}");
        }
    }
}
