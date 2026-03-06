using UnityEngine;

public class IntegrityTestRunner : MonoBehaviour
{
    void Start()
    {
        // Ensure all managers are available
        if (IntegrityManager.Instance == null || PlayerCoinManager.Instance == null || SaveManager.Instance == null || RareDropManager.Instance == null)
        {
            Debug.LogError("IntegrityTestRunner: Missing required managers for testing.");
            return;
        }

        Debug.Log("--- STARTING INTEGRITY SYSTEM STRESS TEST ---");

        // Test 1: Simulate 200 valid runs
        SimulateValidRuns(200);

        // Test 2: Simulate 10 rare drops with valid probabilities
        SimulateRareDrops(10);

        // Test 3: Simulate multiple revive uses within limits
        SimulateReviveUsage();

        // Test 4: Simulate currency purchases
        SimulateCurrencyTransactions();

        // Test 5: Simulate a save/load cycle
        SimulateSaveLoadCycle();

        Debug.Log("--- INTEGRITY SYSTEM STRESS TEST COMPLETE ---");
    }

    private void SimulateValidRuns(int runCount)
    {
        Debug.Log($"--- Simulating {runCount} Valid Runs ---");
        for (int i = 0; i < runCount; i++)
        {
            var runData = new RunSessionData
            {
                score = Random.Range(1000, 50000),
                distance = Random.Range(500, 10000),
                highestMultiplier = Random.Range(1, 8),
                reviveCount = 0
            };

            if (!IntegrityManager.Instance.sessionValidator.ValidateRun(runData, IntegrityManager.Instance.maxRevivesPerRun))
            {
                Debug.LogError($"[FAIL] False positive detected on valid run simulation #{i + 1}");
            }
            else
            {
                //Debug.Log($"[PASS] Run #{i + 1} validated successfully.");
            }
        }
        Debug.Log("--- Valid Run Simulation Complete ---");
    }

    private void SimulateRareDrops(int dropCount)
    {
        Debug.Log($"--- Simulating {dropCount} Valid Rare Drops ---");
        for (int i = 0; i < dropCount; i++)
        {
            var dropData = new RareDropData { dropName = $"SimulatedDrop{i}", baseChance = 0.1f, pityIncrease = 0.02f };
            int pityCounter = Random.Range(0, 10);

            if (!IntegrityManager.Instance.economyValidator.ValidateRareDrop(dropData, pityCounter))
            {
                Debug.LogError($"[FAIL] False positive detected on valid rare drop simulation #{i + 1}");
            }
            else
            {
                //Debug.Log($"[PASS] Rare Drop #{i + 1} validated successfully.");
            }
        }
        Debug.Log("--- Rare Drop Simulation Complete ---");
    }

    private void SimulateReviveUsage()
    {
        Debug.Log("--- Simulating Valid Revive Usage ---");
        var runData = new RunSessionData { reviveCount = 1 };
        if (!IntegrityManager.Instance.sessionValidator.ValidateRun(runData, IntegrityManager.Instance.maxRevivesPerRun))
        {
            Debug.LogError("[FAIL] False positive detected on valid revive usage.");
        }
        else
        {
            Debug.Log("[PASS] Valid revive usage validated successfully.");
        }

        Debug.Log("--- Simulating INVALID Revive Usage ---");
        var invalidRunData = new RunSessionData { reviveCount = 2 };
        if (IntegrityManager.Instance.sessionValidator.ValidateRun(invalidRunData, IntegrityManager.Instance.maxRevivesPerRun))
        {
            Debug.LogError("[FAIL] Failed to detect invalid revive usage.");
        }
        else
        {
            Debug.Log("[PASS] Invalid revive usage correctly detected.");
        }
        Debug.Log("--- Revive Usage Simulation Complete ---");
    }

    private void SimulateCurrencyTransactions()
    {
        Debug.Log("--- Simulating Valid Currency Transactions ---");
        int initialCoins = 1000;
        PlayerCoinManager.Instance.SetTotalCoins(initialCoins);

        // 1. Earn coins
        PlayerCoinManager.Instance.UpdateCoins(500);
        if (PlayerCoinManager.Instance.GetTotalCoins() != 1500)
        {
            Debug.LogError("[FAIL] Coin earning transaction failed.");
        }
        else
        {
            Debug.Log("[PASS] Coin earning validated.");
        }

        // 2. Spend coins
        PlayerCoinManager.Instance.SpendCoins(200);
        if (PlayerCoinManager.Instance.GetTotalCoins() != 1300)
        {
            Debug.LogError("[FAIL] Coin spending transaction failed.");
        }
        else
        {
            Debug.Log("[PASS] Coin spending validated.");
        }

        Debug.Log("--- Simulating INVALID Currency Transactions ---");
        // 3. Try to spend more coins than available (should be blocked)
        bool spendResult = PlayerCoinManager.Instance.SpendCoins(2000);
        if (spendResult || PlayerCoinManager.Instance.GetTotalCoins() != 1300)
        {
            Debug.LogError("[FAIL] Invalid spending was not blocked by the system.");
        }
        else
        {
            Debug.Log("[PASS] Invalid spending correctly blocked.");
        }
        Debug.Log("--- Currency Transaction Simulation Complete ---");
    }

    private void SimulateSaveLoadCycle()
    {
        Debug.Log("--- Simulating Save/Load Cycle ---");
        
        // 1. Set some data and save
        SaveManager.Instance.GameData.playerMetaData.playerLevel = 10;
        SaveManager.Instance.GameData.playerMetaData.coins = 555;
        SaveManager.Instance.SaveGame();
        Debug.Log("Initial save complete.");

        // 2. Load the game and verify data
        SaveManager.Instance.GameData.playerMetaData.playerLevel = 1; // Reset before load
        SaveManager.Instance.GameData.playerMetaData.coins = 0;
        SaveManager.Instance.LoadGame();

        if (SaveManager.Instance.GameData.playerMetaData.playerLevel == 10 && SaveManager.Instance.GameData.playerMetaData.coins == 555)
        {
            Debug.Log("[PASS] Data loaded correctly from valid save file.");
        }
        else
        {
            Debug.LogError("[FAIL] Data mismatch after loading from a valid save.");
        }

        Debug.Log("--- Corrupted Save Simulation ---");
        // 3. Manually "corrupt" the save file (by creating a mismatch) and try to load
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "gameSave.json");
        System.IO.File.WriteAllText(savePath, "{ \"playerMetaData\": { \"playerLevel\": 999, \"coins\": -500 } }"); // Invalid data

        SaveManager.Instance.LoadGame();

        // Check if the loaded data was restored from the backup, not the corrupt file
        if (SaveManager.Instance.GameData.playerMetaData.playerLevel == 10 && SaveManager.Instance.GameData.playerMetaData.coins == 555)
        {
            Debug.Log("[PASS] Corrupted save detected and data successfully restored from backup.");
        }
        else
        {
            Debug.LogError("[FAIL] System failed to restore data from backup after detecting corruption.");
        }

        Debug.Log("--- Save/Load Simulation Complete ---");
    }
}
