
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
        int coinsEarned = (int)(runData.distance / 10); // 1 coin for every 10 meters
        Award("COINS", coinsEarned);

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
        // INTEGRATION: Validate the reward before granting it.
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.GrantReward(itemID))
        {
            IntegrityManager.Instance.ReportError($"Duplicate reward detected: {itemID}");
            return;
        }

        // This method is the designated authority for granting items.
        // It is called by RareDropManager and other systems.
        Debug.Log($"REWARD_MANAGER: Awarding {quantity} of {itemID}");

        if (itemID.StartsWith("SHARD_"))
        {
            // Note: RareDropManager now calls ShardInventoryManager directly for shard awards.
            // This route can be maintained for other shard sources (e.g., chests).
            if (ShardInventoryManager.Instance != null) ShardInventoryManager.Instance.AddShards(itemID, quantity); // Ensure correct method name
        }
        else if (itemID.StartsWith("SKIN_"))
        {
            if (SkinManager.Instance != null) SkinManager.Instance.UnlockSkin(itemID);
        }
        else if (itemID.StartsWith("EFFECT_"))
        {
            if (CosmeticEffectManager.Instance != null) CosmeticEffectManager.Instance.UnlockEffect(itemID);
        }
        else if (itemID == "COINS")
        {
            if (PlayerCoinManager.Instance != null) PlayerCoinManager.Instance.UpdateCoins(quantity);
        }
        else
        {
            Debug.LogWarning($"No manager found to handle reward: {itemID}");
        }
    }

    public void AwardChallengeReward()
    {
        int coinReward = 500;
        // Double rewards during events
        if (LiveOpsManager.Instance != null && LiveOpsManager.Instance.IsEventActive("Friend Rivalry Week"))
        {
            coinReward *= 2;
        }
        Award("COINS", coinReward);
        // In a real game, we would award challenge tokens and chests here.
        Debug.Log("Awarding challenge reward!");
    }
}
