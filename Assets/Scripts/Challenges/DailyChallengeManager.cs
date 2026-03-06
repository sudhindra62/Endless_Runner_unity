
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A potential modifier for the daily challenge.
/// </summary>
public enum ChallengeModifier
{
    None,
    LowGravity,
    DoubleCoins,
    HighSpeed,
    FogMode,
    NoMagnet
}

/// <summary>
/// Manages the state and flow of the Daily Challenge game mode.
/// Integrates with various systems to start the challenge with the correct seed and modifiers.
/// </summary>
public class DailyChallengeManager : Singleton<DailyChallengeManager>
{
    public ChallengeModifier CurrentModifier { get; private set; }
    public bool IsChallengeActive { get; private set; }

    private void Start()
    {
        // Determine today's modifier based on the seed, ensuring it's the same for all players.
        DetermineDailyModifier();
    }

    private void DetermineDailyModifier()
    {
        // Use the day's seed to deterministically pick a modifier.
        int seed = DailyChallengeSeedGenerator.GetCurrentDaySeed();
        System.Random random = new System.Random(seed);
        
        // Get all possible modifier values
        var modifiers = System.Enum.GetValues(typeof(ChallengeModifier));
        // Pick a random one, skipping 'None'
        CurrentModifier = (ChallengeModifier)modifiers.GetValue(random.Next(1, modifiers.Length));
        Debug.Log($"Today's Daily Challenge modifier is: {CurrentModifier}");
    }

    /// <summary>
    /// Starts the Daily Challenge run.
    /// </summary>
    public void StartDailyChallenge()
    {
        if (!ChallengeAttemptTracker.Instance.HasAttemptsRemaining())
        {
            Debug.LogWarning("Cannot start Daily Challenge: No attempts remaining.");
            // Optionally, show a UI prompt to buy another attempt.
            return;
        }

        IsChallengeActive = true;
        ChallengeAttemptTracker.Instance.RecordAttempt();

        int seed = DailyChallengeSeedGenerator.GetCurrentDaySeed();

        // The GameFlowController and ProceduralPatternEngine would need to be designed
        // to accept a seed and modifiers when starting a run.
        
        // Example Integration:
        // GameFlowController.Instance.StartRun(new RunParameters
        // {
        //     IsDailyChallenge = true,
        //     Seed = seed,
        //     Modifier = CurrentModifier 
        // });
        
        Debug.Log($"Starting Daily Challenge with Seed: {seed} and Modifier: {CurrentModifier}");
    }

    /// <summary>
    /// Called when a Daily Challenge run ends.
    /// </summary>
    public void EndDailyChallenge(int finalScore)
    {
        if (!IsChallengeActive) return;
        IsChallengeActive = false;

        string challengeId = DailyChallengeSeedGenerator.GetCurrentDayChallengeId();
        
        // The LeaderboardManager would need a method to submit scores to a specific daily board.
        // LeaderboardManager.Instance.SubmitDailyChallengeScore(challengeId, finalScore);
        
        Debug.Log($"Daily Challenge ended. Final Score: {finalScore}. Submitting to board: {challengeId}");
    }
}
