using UnityEngine;

public class RareDropManager : Singleton<RareDropManager>
{
    // ... (existing code) ...

    public void EvaluateRareDrop(RunSessionData runData, bool bossDefeated)
    {
        // ... (existing integrity checks) ...

        // STEP 3: Standard Drop Calculation
        float baseChance = CalculateBaseChance(runData, bossDefeated);
        float finalChance = ApplyBoostsAndModifiers(baseChance);

        // --- DATA INTEGRITY VALIDATION ---
        RareDropData simulatedDropData = new RareDropData { baseChance = finalChance, pityIncrease = 0.01f }; // Create a temporary drop data for validation
        int pityCounter = (PityCounterManager.Instance != null) ? PityCounterManager.Instance.GetPityCount("Legendary") : 0;
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateRareDrop(simulatedDropData, pityCounter))
        {
            IntegrityManager.Instance.ReportError("Rare drop validation failed for the current evaluation!");
            return;
        }

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

    // ... (rest of the existing code) ...
}
