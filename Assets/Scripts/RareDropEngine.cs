using UnityEngine;
using System;

public class RareDropEngine : MonoBehaviour
{
    public static event Action<string, string> OnRareDropAwarded; // string: itemID, string: rarity

    [Header("Dependencies")]
    [SerializeField] private DropTableRegistry dropTableRegistry;
    [SerializeField] private PityCounterManager pityCounterManager;
    [SerializeField] private DropIntegrityValidator dropIntegrityValidator;
    [SerializeField] private RunSessionData runSessionData; // Assuming this holds all run-specific data

    [Header("Multipliers")]
    private float eventBoostMultiplier = 1.0f;
    private float leagueBoostMultiplier = 1.0f;

    private const float MAX_BOOST_CAP = 2.5f;

    private void OnEnable()
    {
        // Subscribe to events from other managers to update multipliers
        LiveEventManager.OnEventBoostChanged += UpdateEventMultiplier;
        LeagueManager.OnLeagueBoostChanged += UpdateLeagueMultiplier;
    }

    private void OnDisable()
    {
        LiveEventManager.OnEventBoostChanged -= UpdateEventMultiplier;
        LeagueManager.OnLeagueBoostChanged -= UpdateLeagueMultiplier;
    }

    public void EvaluateDrop()
    {
        if (!dropIntegrityValidator.IsRunValid(runSessionData))
        {
            Debug.LogWarning("Rare Drop evaluation cancelled due to invalid run data.");
            return;
        }

        // --- PITY SYSTEM ---
        if (pityCounterManager.CheckAndGrantPityDrop())
        {
            return; // Pity drop was granted, so we skip the normal evaluation.
        }

        // --- STANDARD DROP CALCULATION ---
        float baseChance = CalculateBaseChance();
        float finalChance = ApplyBoostsAndModifiers(baseChance);

        // --- ROLL THE DICE ---
        if (UnityEngine.Random.Range(0f, 1f) <= finalChance)
        {
            AwardRandomDrop();
        }
    }

    private float CalculateBaseChance()
    {
        // A complex formula considering various run factors
        float distanceFactor = Mathf.Clamp01(runSessionData.distance / 1000f); // Example: 1 point per 1000m
        float styleFactor = Mathf.Clamp01(runSessionData.styleScore / 5000f); // Example: 1 point per 5000 style pts
        float comboFactor = Mathf.Clamp01(runSessionData.comboPeak / 100f); // Example: 1 point per 100x combo
        float riskLaneFactor = Mathf.Clamp01(runSessionData.riskLaneUsage / 10f); // Example: 1 point per 10s in risk lane

        // Combine factors (this is a simplified example)
        float combinedFactor = (distanceFactor + styleFactor + comboFactor + riskLaneFactor) / 4f;

        // The base chance is low and should be carefully balanced
        return Mathf.Lerp(0.001f, 0.05f, combinedFactor); // 0.1% to 5% base chance
    }

    private float ApplyBoostsAndModifiers(float baseChance)
    {
        float totalBoost = Mathf.Clamp(eventBoostMultiplier + leagueBoostMultiplier, 1.0f, MAX_BOOST_CAP);
        // We could add difficulty modifiers here as well
        return baseChance * totalBoost;
    }

    private void AwardRandomDrop()
    {
        DropTableRegistry.DropItem itemToAward = dropTableRegistry.GetRandomItem("Default");

        if (itemToAward != null)
        {
            string itemID = itemToAward.itemID;
            string rarity = itemToAward.rarityProfile.rarityName;

            Debug.Log($"<color=purple>RARE DROP AWARDED! Item: {itemID}, Rarity: {rarity}</color>");

            RewardManager.Instance.Award(itemID, 1);

            OnRareDropAwarded?.Invoke(itemID, rarity);
            pityCounterManager.ResetPityCounter(rarity);
        }
    }

    private void UpdateEventMultiplier(float newEventBoost)
    {
        eventBoostMultiplier = newEventBoost;
    }

    private void UpdateLeagueMultiplier(float newLeagueBoost)
    {
        leagueBoostMultiplier = newLeagueBoost;
    }

    // This would be called by a LiveEventManager or LeagueManager
    public void UpdateMultipliers(float newEventBoost, float newLeagueBoost)
    {
        eventBoostMultiplier = newEventBoost;
        leagueBoostMultiplier = newLeagueBoost;
    }
}
