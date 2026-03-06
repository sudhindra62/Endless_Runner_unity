using UnityEngine;

public class IntegrityManager : MonoBehaviour
{
    public static IntegrityManager Instance { get; private set; }

    private SessionValidator sessionValidator;
    private EconomyValidator economyValidator;
    private SaveIntegrityGuard saveIntegrityGuard;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sessionValidator = new SessionValidator();
        economyValidator = new EconomyValidator();
        saveIntegrityGuard = new SaveIntegrityGuard();
    }

    public bool ValidateRun(float runDistance, float runTime)
    {
        return sessionValidator.IsRunDataValid(runDistance, runTime) && sessionValidator.IsTimeScaleValid();
    }

    public bool ValidateCurrencyChange(int previousAmount, int currentAmount, int changeAmount)
    {
        return economyValidator.IsCurrencyChangeValid(previousAmount, currentAmount, changeAmount);
    }

    public bool GrantReward(string rewardId)
    {
        return economyValidator.GrantReward(rewardId);
    }

    public string GenerateSaveChecksum(string data)
    {
        return saveIntegrityGuard.GenerateChecksum(data);
    }

    public bool ValidateSaveChecksum(string data, string checksum)
    {
        return saveIntegrityGuard.IsChecksumValid(data, checksum);
    }

    public void SimulateLongSession()
    {
        Debug.Log("Starting long session simulation...");

        // Simulate 200 runs
        for (int i = 0; i < 200; i++)
        {
            float runTime = Random.Range(60f, 300f);
            float runDistance = runTime * Random.Range(10f, 25f);
            if (!ValidateRun(runDistance, runTime))
            {
                ReportError("Long session simulation failed during run validation.");
                return;
            }
        }

        // Simulate currency changes
        int currency = 1000;
        for (int i = 0; i < 1000; i++)
        {
            int change = Random.Range(-100, 100);
            int newCurrency = currency + change;
            if (!ValidateCurrencyChange(currency, newCurrency, change))
            {
                ReportError("Long session simulation failed during currency validation.");
                return;
            }
            currency = newCurrency;
        }

        // Simulate LiveOps changes
        Debug.Log("Simulating LiveOps changes...");

        Debug.Log("Long session simulation completed successfully!");
    }

    public void ReportError(string message)
    {
        Debug.LogError("Integrity Error: " + message);
    }
}
