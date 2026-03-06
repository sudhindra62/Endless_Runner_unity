
using UnityEngine;

/// <summary>
/// Validates and clamps LiveOps configuration profiles.
/// </summary>
public static class LiveOpsSafetyValidator
{
    /// <summary>
    /// Validates and clamps a fetched LiveOps configuration profile against a safe default profile.
    /// </summary>
    /// <param name="fetchedProfile">The fetched profile to validate.</param>
    /// <param name="safeDefaultProfile">The safe default profile to use for clamping.</param>
    /// <returns>The validated and clamped profile.</returns>
    public static LiveOpsConfigProfile ValidateAndClamp(LiveOpsConfigProfile fetchedProfile, LiveOpsConfigProfile safeDefaultProfile)
    {
        // In a real implementation, you would have more sophisticated validation rules.
        // For this example, we'll just clamp the values to a safe range.
        LiveOpsConfigProfile validatedProfile = ScriptableObject.CreateInstance<LiveOpsConfigProfile>();

        validatedProfile.difficultyMultiplier = Mathf.Clamp(fetchedProfile.difficultyMultiplier, 0.5f, 2.0f);
        validatedProfile.powerUpDurationMultiplier = Mathf.Clamp(fetchedProfile.powerUpDurationMultiplier, 0.5f, 2.0f);
        validatedProfile.dropRateMultiplier = Mathf.Clamp(fetchedProfile.dropRateMultiplier, 0.5f, 2.0f);
        validatedProfile.riskLaneRewardMultiplier = Mathf.Clamp(fetchedProfile.riskLaneRewardMultiplier, 0.5f, 2.0f);
        validatedProfile.reviveGemCost = Mathf.Clamp(fetchedProfile.reviveGemCost, 5, 20);
        validatedProfile.isEventActive = fetchedProfile.isEventActive;

        return validatedProfile;
    }
}
