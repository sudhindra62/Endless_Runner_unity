/// <summary>
/// Defines all supported mission types.
/// NOTE: Values are added ONLY to restore compatibility with existing code.
/// </summary>
public enum MissionType
{
    // Collect a certain number of coins
    CollectCoins,

    // Run a specific distance in a single run
    RunDistance,

    // Survive for a specific amount of time in a single run
    SurviveTime,

    // 🔹 REQUIRED: referenced by MissionProgressTracker
    JumpTotal
}
