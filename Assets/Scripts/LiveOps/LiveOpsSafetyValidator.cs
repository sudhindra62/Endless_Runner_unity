
using UnityEngine;

/// <summary>
/// Provides static methods for validating and clamping LiveOps configuration values against a safety profile.
/// Refactored by the Supreme Guardian Architect v12 to be fully configurable by designers.
/// </summary>
public static class LiveOpsSafetyValidator
{
    /// <summary>
    /// Takes a raw config profile and returns a sanitized, safe-to-use version based on a bounds asset.
    /// </summary>
    /// <param name="rawProfile">The profile fetched from a remote source or cache.</param>
    /// <param name="safetyBounds">The ScriptableObject defining the min/max for each value.</param>
    /// <param name="safeDefault">A guaranteed-safe profile to use for fallback values if validation fails.</param>
    /// <returns>A validated and clamped LiveOpsConfigProfile instance.</returns>
    public static LiveOpsConfigProfile ValidateAndClamp(LiveOpsConfigProfile rawProfile, LiveOpsSafetyBounds safetyBounds, LiveOpsConfigProfile safeDefault)
    {
        if (rawProfile == null)
        {
            Debug.LogWarning("LiveOps VALIDATION: Raw config profile was null. Falling back to safe defaults.");
            return safeDefault;
        }

        if (safetyBounds == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: No LiveOpsSafetyBounds provided. Cannot validate. Returning safe default.");
            return safeDefault;
        }

        LiveOpsConfigProfile validatedProfile = ScriptableObject.CreateInstance<LiveOpsConfigProfile>();

        // --- Validate and Clamp Each Parameter Using the Safety Bounds ---
        validatedProfile.difficultyMultiplier = Clamp(rawProfile.difficultyMultiplier, safetyBounds.difficultyMultiplier, safeDefault.difficultyMultiplier);
        validatedProfile.powerUpDurationMultiplier = Clamp(rawProfile.powerUpDurationMultiplier, safetyBounds.powerUpDurationMultiplier, safeDefault.powerUpDurationMultiplier);
        validatedProfile.dropRateMultiplier = Clamp(rawProfile.dropRateMultiplier, safetyBounds.dropRateMultiplier, safeDefault.dropRateMultiplier);
        validatedProfile.riskLaneRewardMultiplier = Clamp(rawProfile.riskLaneRewardMultiplier, safetyBounds.riskLaneRewardMultiplier, safeDefault.riskLaneRewardMultiplier);

        validatedProfile.reviveGemCost = Clamp(rawProfile.reviveGemCost, safetyBounds.reviveGemCost, safeDefault.reviveGemCost);
        validatedProfile.adFrequencyModifier = Clamp(rawProfile.adFrequencyModifier, safetyBounds.adFrequencyModifier, safeDefault.adFrequencyModifier);

        // Boolean flags are pass-through as they don't have a range to validate.
        validatedProfile.isEventActive = rawProfile.isEventActive;

        validatedProfile.bossSpawnIntervalMinutes = Clamp(rawProfile.bossSpawnIntervalMinutes, safetyBounds.bossSpawnIntervalMinutes, safeDefault.bossSpawnIntervalMinutes);
        validatedProfile.leagueThresholdAdjustment = Clamp(rawProfile.leagueThresholdAdjustment, safetyBounds.leagueThresholdAdjustment, safeDefault.leagueThresholdAdjustment);

        Debug.Log("<color=cyan>Guardian Architect: LiveOps config validated and clamped against safety bounds.</color>");

        return validatedProfile;
    }

    private static float Clamp(float value, LiveOpsSafetyBounds.FloatRange bounds, float defaultValue)
    {
        if (value < bounds.min || value > bounds.max)
        {
            Debug.LogWarning($"LiveOps CLAMP: Value {value} was outside the safe range [{bounds.min}, {bounds.max}]. Falling back to default {defaultValue}.");
            return defaultValue;
        }
        return value;
    }

    private static int Clamp(int value, LiveOpsSafetyBounds.IntRange bounds, int defaultValue)
    {
        if (value < bounds.min || value > bounds.max)
        {
            Debug.LogWarning($"LiveOps CLAMP: Value {value} was outside the safe range [{bounds.min}, {bounds.max}]. Falling back to default {defaultValue}.");
            return defaultValue;
        }
        return value;
    }
}
