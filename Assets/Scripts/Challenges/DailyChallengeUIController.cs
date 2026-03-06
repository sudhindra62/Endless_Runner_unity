
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the UI panel for the Daily Challenge.
/// Displays challenge details, remaining attempts, and provides the entry point to start the run.
/// </summary>
public class DailyChallengeUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DailyChallengeManager challengeManager;
    [SerializeField] private ChallengeAttemptTracker attemptTracker;
    [SerializeField] private DailyChallengeLeaderboard leaderboard;

    [Header("UI Elements")]
    [SerializeField] private Text modifierText;
    [SerializeField] private Text attemptsText;
    [SerializeField] private Text rewardPreviewText; // To describe rewards
    [SerializeField] private Button startChallengeButton;
    [SerializeField] private Button watchAdForAttemptButton;

    private void OnEnable()
    {
        RefreshPanel();
    }

    /// <summary>
    /// Updates all UI elements with the latest daily challenge information.
    /// </summary>
    public void RefreshPanel()
    {
        if (challengeManager == null || attemptTracker == null || leaderboard == null)
        {
            Debug.LogError("Daily Challenge UI is missing critical dependencies!");
            return;
        }

        // Display the current modifier
        modifierText.text = $"Modifier: {challengeManager.CurrentModifier}";

        // Display remaining attempts
        int attemptsMade = attemptTracker.GetAttemptsMade();
        int maxAttempts = attemptTracker.GetMaxAttempts();
        attemptsText.text = $"Attempts: {maxAttempts - attemptsMade} / {maxAttempts}";

        // Control button visibility
        bool hasAttempts = attemptTracker.HasAttemptsRemaining();
        startChallengeButton.interactable = hasAttempts;
        
        // Show the 'watch ad' button only if the player has used at least one attempt and has no more left.
        watchAdForAttemptButton.gameObject.SetActive(!hasAttempts && attemptsMade > 0);

        // Display reward info (this would be more dynamic in a real game)
        rewardPreviewText.text = "Top 10% get a Rare Chest!\nTop 1% get a Legendary Fragment!";
        
        // Refresh the leaderboard display
        leaderboard.RefreshLeaderboard();
    }

    /// <summary>
    /// Called when the player clicks the 'Start Challenge' button.
    /// </summary>
    public void OnStartChallengePressed()
    {
        challengeManager.StartDailyChallenge();
        // This panel should be closed by the GameFlowController upon successful run start.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the player clicks the 'Watch Ad for Attempt' button.
    /// </summary>
    public void OnWatchAdPressed()
    {
        // --- Ad Monetization Integration --- 
        // AdManager.Instance.ShowRewardedAd(() => {
        //     attemptTracker.AddExtraAttempt();
        //     RefreshPanel(); // Update UI to reflect the new attempt
        // });
        
        Debug.Log("Requesting rewarded ad for extra attempt...");
        // For testing, we can just grant the attempt directly.
        attemptTracker.AddExtraAttempt();
        RefreshPanel();
    }
}
