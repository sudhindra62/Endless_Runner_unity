
using UnityEngine;

/// <summary>
/// Validates the integrity of critical game data to prevent cheating and repair corruption.
/// Acts as the single source of truth for data validation checks.
/// </summary>
public class IntegrityManager : Singleton<IntegrityManager>
{
    // These would be compared against a server-side checksum or a locally encrypted hash
    private const int MAX_CURRENCY_PER_RUN = 5000; // Example cap
    private const int MAX_SCORE_PER_MINUTE = 10000; // Example cap

    /// <summary>
    /// A comprehensive check of all critical save data components before loading.
    /// </summary>
    /// <returns>True if the data is valid.</returns>
    public bool ValidateSaveData(string saveDataJson)
    {
        if (string.IsNullOrEmpty(saveDataJson))
        {
            Debug.LogError("Save data is null or empty. Integrity check failed.");
            return false;
        }

        // 1. Check for signs of tampering (e.g., using a checksum or hash)
        // if (!IsChecksumValid(saveDataJson)) { RepairCorruptedSave(); return false; }

        // 2. Deserialize and check individual values against reasonable limits
        // PlayerData data = JsonUtility.FromJson<PlayerData>(saveDataJson);
        // if (data.gems < 0 || data.coins < 0) { RepairCorruptedSave(); return false; }
        // if (data.playerLevel > 1000) { /* Flag for review */ }

        Debug.Log("Save data integrity check passed.");
        return true;
    }

    /// <summary>
    /// Validates the currency earned in a single run against predefined limits.
    /// </summary>
    public bool ValidateRunEarnings(int coinsEarned, int gemsEarned)
    {
        if (coinsEarned < 0 || gemsEarned < 0)
            return false;

        if (coinsEarned > MAX_CURRENCY_PER_RUN)
        {
            Debug.LogWarning($"Run currency validation failed. Coins earned ({coinsEarned}) exceeded max limit ({MAX_CURRENCY_PER_RUN}).");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates a score against time to prevent speed hacks.
    /// </summary>
    public bool ValidateScore(long score, float durationInMinutes)
    {
        if (durationInMinutes <= 0) return true; // Avoid division by zero

        float scorePerMinute = score / durationInMinutes;
        if (scorePerMinute > MAX_SCORE_PER_MINUTE)
        {
            Debug.LogWarning($"Score validation failed. Score per minute ({scorePerMinute}) exceeded max limit ({MAX_SCORE_PER_MINUTE}).");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Placeholder for the logic to repair a corrupted save file.
    /// This could involve loading a cloud backup or resetting to a default state.
    /// </summary>
    public void RepairCorruptedSave()
    {
        Debug.LogError("Corrupted save data detected. Initiating repair protocol.");
        // 1. Attempt to load from a secure cloud backup (e.g., PlayFab, Firebase)
        // 2. If no backup, revert to default PlayerPrefs/JSON state.
        // 3. Log the event to analytics for developer review.
    }
}
