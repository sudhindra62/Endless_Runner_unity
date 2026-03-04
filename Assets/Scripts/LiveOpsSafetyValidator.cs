
using UnityEngine;

/// <summary>
/// Provides static methods for validating and clamping LiveOps configuration values.
/// Ensures that no corrupt or extreme values can enter the game's systems.
/// Adheres to the principle of Zero Permanent Mutation by operating on transient data.
/// </summary>
public static class LiveOpsSafetyValidator
{
    private const float MAX_GENERAL_MULTIPLIER = 3.0f;
    private const float MAX_DROP_RATE_MULTIPLIER = 1.5f; // Stricter cap for economy-sensitive values

    /// <summary>
    /// Takes a raw config profile and returns a sanitized, safe-to-use version.
    /// </summary>
    /// <param name="rawProfile">The profile fetched from a remote source or cache.</param>
    /// <param name="safeDefault">A guaranteed-safe profile to use for fallback values.</param>
    /// <returns>A validated and clamped LiveOpsConfigProfile.</returns>
    public static LiveOpsConfigProfile ValidateAndClamp(LiveOpsConfigProfile rawProfile, LiveOpsConfigProfile safeDefault)
    {
        if (rawProfile == null)
        {
            Debug.LogWarning("LiveOps VALIDATION: Raw config profile was null. Falling back to safe defaults.");
            return safeDefault;
        }

        LiveOpsConfigProfile validatedProfile = ScriptableObject.CreateInstance<LiveOpsConfigProfile>();

        // --- Validate and Clamp Each Parameter ---

        validatedProfile.difficultyMultiplier = Clamp(rawProfile.difficultyMultiplier, 0.5f, 2.0f, safeDefault.difficultyMultiplier);
        validatedProfile.powerUpDurationMultiplier = Clamp(rawProfile.powerUpDurationMultiplier, 0.5f, MAX_GENERAL_MULTIPLIER, safeDefault.powerUpDurationMultiplier);
        validatedProfile.dropRateMultiplier = Clamp(rawProfile.dropRateMultiplier, 1.0f, MAX_DROP_RATE_MULTIPLIER, safeDefault.dropRateMultiplier);
        validatedProfile.riskLaneRewardMultiplier = Clamp(rawProfile.riskLaneRewardMultiplier, 1.0f, 2.5f, safeDefault.riskLaneRewardMultiplier);

        validatedProfile.reviveGemCost = Clamp(rawProfile.reviveGemCost, 0, 1000, safeDefault.reviveGemCost); // Prevent negative cost
        validatedProfile.adFrequencyModifier = Clamp(rawProfile.adFrequencyModifier, 0.1f, 5.0f, safeDefault.adFrequencyModifier);

        validatedProfile.isEventActive = rawProfile.isEventActive;
        validatedProfile.bossSpawnIntervalMinutes = Clamp(rawProfile.bossSpawnIntervalMinutes, 5.0f, 120.0f, safeDefault.bossSpawnIntervalMinutes); // At least 5 mins
        validatedProfile.leagueThresholdAdjustment = Clamp(rawProfile.leagueThresholdAdjustment, -0.25f, 0.25f, safeDefault.leagueThresholdAdjustment);

        Debug.Log("<color=cyan>LiveOps config validated and clamped successfully.</color>");

        return validatedProfile;
    }

    private static float Clamp(float value, float min, float max, float defaultValue)
    {
        if (value < min || value > max)
        {
            Debug.LogWarning($"LiveOps CLAMP: Value {value} was outside the safe range [{min}, {max}]. Falling back to {defaultValue}.");
            return defaultValue;
        }
        return value;
    }

    private static int Clamp(int value, int min, int max, int defaultValue)
    {
        if (value < min || value > max)
        {
            Debug.LogWarning($"LiveOps CLAMP: Value {value} was outside the safe range [{min}, {max}]. Falling back to {defaultValue}.");
            return defaultValue;
        }
        return value;
    }
}
