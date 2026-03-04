using UnityEngine;
using System;

public class RewardManager : MonoBehaviour
{
    public static event Action OnRewardCalculation;

    [Header("Dependencies")]
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private RareDropEngine rareDropEngine; // The new engine

    // This method is called at the end of a run
    public void CalculateAndAwardRewards(RunSessionData runData)
    {
        // 1. Calculate standard rewards (currency, XP, etc.)
        int currencyEarned = CalculateCurrency(runData);
        currencyManager.AddCurrency(currencyEarned);

        // --- RARE DROP INTEGRATION --- 
        // 2. Trigger the rare drop evaluation BEFORE displaying rewards.
        // The RareDropEngine will listen for this event.
        OnRewardCalculation?.Invoke();

        // 3. Display rewards to the player (UI update)
        DisplayRewardScreen(currencyEarned);
    }

    private int CalculateCurrency(RunSessionData runData)
    {
        // Simplified calculation
        return (int)(runData.distance / 10) + runData.score;
    }

    private void DisplayRewardScreen(int currencyEarned)
    {
        Debug.Log($"<color=cyan>--- END OF RUN REWARDS ---</color>");
        Debug.Log($"Currency Earned: {currencyEarned}");
        // Here, a UI manager would be called to show the end-of-run screen.
    }

    private void OnEnable()
    {
        if(rareDropEngine != null) {
            OnRewardCalculation += rareDropEngine.EvaluateDrop;
        }
    }

    private void OnDisable()
    {
        if(rareDropEngine != null) {
            OnRewardCalculation -= rareDropEngine.EvaluateDrop;
        }
    }
}

// Dummy class for compilation
public class CurrencyManager : MonoBehaviour {
    public void AddCurrency(int amount) {}
}
