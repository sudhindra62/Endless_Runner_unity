using UnityEngine;

/// <summary>
/// Provides static methods for validating and clamping LiveOps configuration values.
/// Global scope for maximum project-wide accessibility.
/// </summary>
public static class LiveOpsSafetyValidator
{
    public static LiveOpsConfigProfile ValidateAndClamp(LiveOpsConfigProfile rawProfile, LiveOpsSafetyBounds safetyBounds, LiveOpsConfigProfile safeDefault)
    {
        if (rawProfile == null) return safeDefault;
        if (safetyBounds == null) return safeDefault;

        LiveOpsConfigProfile validatedProfile = ScriptableObject.CreateInstance<LiveOpsConfigProfile>();

        validatedProfile.difficultyMultiplier = Clamp(rawProfile.difficultyMultiplier, safetyBounds.difficultyMultiplier, safeDefault.difficultyMultiplier);
        validatedProfile.powerUpDurationMultiplier = Clamp(rawProfile.powerUpDurationMultiplier, safetyBounds.powerUpDurationMultiplier, safeDefault.powerUpDurationMultiplier);
        validatedProfile.dropRateMultiplier = Clamp(rawProfile.dropRateMultiplier, safetyBounds.dropRateMultiplier, safeDefault.dropRateMultiplier);
        validatedProfile.riskLaneRewardMultiplier = Clamp(rawProfile.riskLaneRewardMultiplier, safetyBounds.riskLaneRewardMultiplier, safeDefault.riskLaneRewardMultiplier);
        validatedProfile.reviveGemCost = Clamp(rawProfile.reviveGemCost, safetyBounds.reviveGemCost, safeDefault.reviveGemCost);
        validatedProfile.adFrequencyModifier = Clamp(rawProfile.adFrequencyModifier, safetyBounds.adFrequencyModifier, safeDefault.adFrequencyModifier);
        validatedProfile.isEventActive = rawProfile.isEventActive;
        validatedProfile.bossSpawnIntervalMinutes = Clamp(rawProfile.bossSpawnIntervalMinutes, safetyBounds.bossSpawnIntervalMinutes, safeDefault.bossSpawnIntervalMinutes);
        validatedProfile.leagueThresholdAdjustment = Clamp(rawProfile.leagueThresholdAdjustment, safetyBounds.leagueThresholdAdjustment, safeDefault.leagueThresholdAdjustment);

        return validatedProfile;
    }

    private static float Clamp(float value, LiveOpsSafetyBounds.FloatRange bounds, float defaultValue)
    {
        if (value < bounds.min || value > bounds.max) return defaultValue;
        return value;
    }

    private static int Clamp(int value, LiveOpsSafetyBounds.IntRange bounds, int defaultValue)
    {
        if (value < bounds.min || value > bounds.max) return defaultValue;
        return value;
    }
}
