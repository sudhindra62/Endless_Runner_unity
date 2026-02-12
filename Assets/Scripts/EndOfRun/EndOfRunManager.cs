
using UnityEngine;

/// <summary>
/// An orchestrator singleton that manages the entire end-of-run process.
/// It uses the other EndOfRun components to build, calculate, and apply rewards.
/// 
/// --- INTEGRATION ---
/// This component should be placed on a persistent GameManager object.
/// The existing GameManager should call this singleton's `ProcessEndOfRun` method when the game is over.
/// Example: `EndOfRunManager.Instance.ProcessEndOfRun();`
/// </summary>
[RequireComponent(typeof(RunSummaryBuilder), typeof(EndRunRewardApplier), typeof(BonusMultiplierHandler))]
public class EndOfRunManager : MonoBehaviour
{
    public static EndOfRunManager Instance { get; private set; }

    [SerializeField] private RunSummaryBuilder summaryBuilder;
    [SerializeField] private EndRunRewardApplier rewardApplier;
    [SerializeField] private BonusMultiplierHandler multiplierHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// The main entry point to be called by the GameManager when a run ends.
    /// This orchestrates the entire process of summarizing, calculating, and applying rewards.
    /// </summary>
    public void ProcessEndOfRun()
    {
        // 1. Build the initial summary from the run data.
        RunSummaryData summary = summaryBuilder.BuildSummary();

        // 2. Get the current multiplier (defaults to 1.0).
        summary.bonusMultiplier = multiplierHandler.GetCurrentMultiplier();

        // 3. Calculate final rewards.
        var rewardCalculator = new EndRunRewardCalculator(summary);
        var (finalCoins, finalXP) = rewardCalculator.CalculateFinalRewards();

        // 4. Apply the rewards to the player's persistent data.
        rewardApplier.ApplyRewards(finalCoins, finalXP);

        // 5. Reset the multiplier for the next run.
        multiplierHandler.ResetMultiplier();

        Debug.Log($"End of Run Process Complete. Rewarded {finalCoins} coins and {finalXP} XP.");

        // FUTURE HOOK: The End-Run UI system will be triggered from here, passing the `summary` object.
        // e.g., EndRunUIManager.Instance.ShowSummaryScreen(summary);
    }
}
