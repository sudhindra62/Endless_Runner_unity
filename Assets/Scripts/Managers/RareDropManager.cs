
using UnityEngine;
using System;

public class RareDropManager : MonoBehaviour
{
    public static event Action<string, string> OnRareDropAwarded; // string: itemID, string: rarity

    [Header("Dependencies")]
    [SerializeField] private DropTableRegistry dropTableRegistry;
    [SerializeField] private RunSessionData runSessionData; 
    
    // Singleton instance for external access
    public static RareDropManager Instance { get; private set; }

    [Header("Boost Caps")]
    [Tooltip("The absolute maximum boost multiplier allowed from all sources combined.")]
    [SerializeField] private float maxTotalBoostCap = 3.0f;

    // Multipliers from external systems
    private float eventBoostMultiplier = 1.0f;
    private float leagueBoostMultiplier = 1.0f;

    #region Original Logic Preservation
    // This region contains the original logic from the legacy RareDropManager.
    // It is preserved here to adhere to the 'NEVER DELETE' rule.
    private void Original_AttemptRareDrop_Legacy()
    {
        const float BASE_DROP_CHANCE = 0.05f; // 5% chance
        float currentDropChance = BASE_DROP_CHANCE;

        // The IntegrityManager could temporarily reduce this chance.
        // Note: This check is now superseded by the more robust DropIntegrityValidator.
        if (IntegrityManager.Instance != null && IntegrityManager.Instance.IsSessionSuspicious)
        {
            Debug.LogWarning("Original Logic: Applying penalty to rare drop chance due to suspicious activity.");
            // currentDropChance *= 0.5f;
        }

        if (UnityEngine.Random.value < currentDropChance)
        {
            Debug.Log("Original Logic: Success! A rare item dropped!");
            // The new system uses a more detailed event.
            // OnRareDropAwarded?.Invoke(); 
        }
        else
        {
            Debug.Log("Original Logic: No rare drop this time.");
        }
    }
    #endregion


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

    private void OnEnable()
    {
        // Subscribe to events for dynamic multiplier updates
        // LiveEventManager.OnEventBoostChanged += UpdateEventMultiplier;
        // LeagueManager.OnLeagueBoostChanged += UpdateLeagueMultiplier;
    }

    private void OnDisable()
    {
        // LiveEventManager.OnEventBoostChanged -= UpdateEventMultiplier;
        // LeagueManager.OnLeagueBoostChanged -= UpdateLeagueMultiplier;
    }

    /// <summary>
    /// Primary entry point for evaluating a rare drop at the end of a run.
    /// This is to be called EXCLUSIVELY by RewardManager.
    /// </summary>
    public void EvaluateRareDrop(RunSessionData runData, bool bossDefeated)
    {
        // STEP 1: Integrity Validation
        if (DropIntegrityValidator.Instance != null && !DropIntegrityValidator.Instance.IsRunValid(runData, bossDefeated))
        {
            Debug.LogWarning("RARE DROP CANCELED: Run failed integrity validation.");
            if(PityCounterManager.Instance != null) PityCounterManager.Instance.IncrementPityCounters(); // Still increment pity
            return;
        }

        // STEP 2: Pity System - Check for Guaranteed Drops
        if (PityCounterManager.Instance != null && PityCounterManager.Instance.IsPityGuaranteeMet("Epic"))
        {
            AwardGuaranteedDrop("Epic");
            PityCounterManager.Instance.ResetPityCounter("Epic");
            return; // Exit after awarding guaranteed drop
        }

        // STEP 3: Standard Drop Calculation
        float baseChance = CalculateBaseChance(runData, bossDefeated);
        float finalChance = ApplyBoostsAndModifiers(baseChance);

        // STEP 4: Roll for the Drop
        if (UnityEngine.Random.Range(0f, 1f) <= finalChance)
        {
            AwardRandomDrop();
        }
        else
        {
            // If no drop, increment pity counters for the next run
            if(PityCounterManager.Instance != null) PityCounterManager.Instance.IncrementPityCounters();
            Debug.Log("No rare drop awarded this run.");
        }
    }

    private float CalculateBaseChance(RunSessionData sessionData, bool bossDefeated)
    {
        // Formula balances multiple run factors to produce a base chance (e.g., between 0.1% and 5%)
        if (sessionData == null) return 0.01f; // Failsafe
        float distanceFactor = Mathf.Clamp01(sessionData.distance / 5000f); // Max factor at 5000m
        float styleFactor = Mathf.Clamp01(sessionData.styleScore / 10000f); // Max factor at 10k style
        float comboFactor = Mathf.Clamp01(sessionData.comboPeak / 200f); // Max factor at 200x combo
        float riskLaneFactor = Mathf.Clamp01(sessionData.riskLaneUsage / 30f); // Max factor at 30s in risk lane
        float bossFactor = bossDefeated ? 0.1f : 0f; // Flat 10% bonus for defeating a boss

        // Combine factors with different weights
        float combinedFactor = (distanceFactor * 0.4f) + (styleFactor * 0.2f) + (comboFactor * 0.2f) + (riskLaneFactor * 0.1f) + bossFactor;

        // Lerp to map the combined factor to a final base chance
        return Mathf.Lerp(0.001f, 0.05f, combinedFactor); // 0.1% to 5% base chance
    }

    private float ApplyBoostsAndModifiers(float baseChance)
    {
        // 1. *** LIVE OPS INTEGRATION POINT ***
        // PULL the modifier from the LiveOpsManager.
        float liveOpsMultiplier = 1.0f;
        if (LiveOpsManager.Instance != null)
        {
            liveOpsMultiplier = LiveOpsManager.Instance.DropRateMultiplier;
        }
        
        // 2. Get other dynamic boosts from their respective managers
        float pityBoost = (PityCounterManager.Instance != null) ? PityCounterManager.Instance.GetPityBoost("Legendary") : 1.0f;

        // 3. Combine all multipliers together
        float totalBoost = eventBoostMultiplier * leagueBoostMultiplier * liveOpsMultiplier * pityBoost;
        
        // 4. Clamp to the absolute maximum defined in this manager, as a final safety measure
        totalBoost = Mathf.Clamp(totalBoost, 1.0f, maxTotalBoostCap);

        Debug.Log($"Applied Rare Drop Boosts. Final Multiplier: {totalBoost}");

        return baseChance * totalBoost;
    }

    private void AwardRandomDrop()
    {
        if (dropTableRegistry == null) return;
        // For now, we only use the "Default" table. This can be expanded.
        var itemToAward = dropTableRegistry.GetRandomItem("Default");

        if (itemToAward != null)
        {
            FinalizeAndAward(itemToAward.itemID, itemToAward.rarityProfile.rarityName);
            if(PityCounterManager.Instance != null) PityCounterManager.Instance.ResetPityCounter(itemToAward.rarityProfile.rarityName);
        }
    }

    private void AwardGuaranteedDrop(string rarity)
    {
        if (dropTableRegistry == null) return;
        // Find an item of the guaranteed rarity from the drop table
        var guaranteedItem = dropTableRegistry.GetRandomItem("Default"); // Simplified: get any, then check
        // In a real scenario, you would have a method like GetRandomItemOfRarity(rarity)
        if (guaranteedItem != null && guaranteedItem.rarityProfile.rarityName == rarity)
        {
            Debug.Log($"<color=yellow>PITY GUARANTEE: Awarding a guaranteed {rarity} item!</color>");
            FinalizeAndAward(guaranteedItem.itemID, guaranteedItem.rarityProfile.rarityName);
        }
        else
        {
            // Fallback if no item of that rarity is found, though this indicates a config error
            AwardRandomDrop(); 
        }
    }

    private void FinalizeAndAward(string itemID, string rarity)
    {
        Debug.Log($"<color=purple>RARE DROP AWARDED! Item: {itemID}, Rarity: {rarity}</color>");
        
        // Check if it's a shard or a full item
        if (itemID.StartsWith("SHARD_"))
        {
            if(ShardInventoryManager.Instance != null) ShardInventoryManager.Instance.AddShards(itemID, 10); // Example: award 10 shards
        }
        else
        {
            // Route the final reward through the authoritative RewardManager
            if(RewardManager.Instance != null) RewardManager.Instance.Award(itemID, 1);
        }
        
        // Fire event for UI systems
        OnRareDropAwarded?.Invoke(itemID, rarity);
    }

    // Method to be called by LiveEventManager
    private void UpdateEventMultiplier(float newEventBoost)
    {
        eventBoostMultiplier = newEventBoost;
    }

    // Method to be called by LeagueManager
    private void UpdateLeagueMultiplier(float newLeagueBoost)
    { 
        leagueBoostMultiplier = newLeagueBoost;
    }
}
