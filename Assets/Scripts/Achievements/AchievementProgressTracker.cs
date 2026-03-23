
using UnityEngine;


/// <summary>
/// Tracks various gameplay stats and reports progress to the AchievementManager.
/// This script acts as the bridge between gameplay events and the achievement system.
/// </summary>
public class AchievementProgressTracker : MonoBehaviour
{
    private void Start()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to relevant game events ---
        // Example subscriptions:
        // if (ScoreManager.Instance != null) ScoreManager.OnScoreChanged += HandleScoreChange;
        // if (PlayerStats.Instance != null) PlayerStats.OnDistanceChanged += HandleDistanceChange;
    }

    private void OnDestroy()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks ---
        // Example unsubscriptions:
        // if (ScoreManager.Instance != null) ScoreManager.OnScoreChanged -= HandleScoreChange;
        // if (PlayerStats.Instance != null) PlayerStats.OnDistanceChanged -= HandleDistanceChange;
    }

    // Example handler for score changes
    private void HandleScoreChange(int newScore)
    {
        AchievementManager.Instance.AddProgress(AchievementType.Score, newScore);
    }

    // Example handler for distance changes
    private void HandleDistanceChange(float newDistance)
    {
        AchievementManager.Instance.AddProgress(AchievementType.Distance, (int)newDistance);
    }
    
    // Add more handlers for other events like coin collection, power-up usage, etc.
}
