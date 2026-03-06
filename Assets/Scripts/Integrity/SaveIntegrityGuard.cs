using UnityEngine;

/// <summary>
/// Manages the snapshotting and restoration of save data to prevent corruption.
/// </summary>
public class SaveIntegrityGuard
{
    private SaveData lastKnownGoodSave;
    private string backupSaveSlot = "BackupSaveData";

    /// <summary>
    /// Creates a snapshot of the current save data before a new save operation.
    /// </summary>
    /// <param name="currentSaveData">The current state of the game data.</param>
    public void CreateBackup(SaveData currentSaveData)
    {
        // In a real implementation, we would create a deep copy.
        // For this structure, we'll assume SaveData is a class and can be referenced.
        lastKnownGoodSave = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(currentSaveData));
        PlayerPrefs.SetString(backupSaveSlot, JsonUtility.ToJson(lastKnownGoodSave));
        Debug.Log("[SaveIntegrityGuard] Created a backup of the current save data.");
    }

    /// <summary>
    /// Restores the last known good save data.
    /// This is called when corruption is detected in the main save file.
    /// </summary>
    /// <returns>The restored SaveData object.</returns>
    public SaveData RestoreBackup()
    {
        if (lastKnownGoodSave != null)
        {
            Debug.LogWarning("[SaveIntegrityGuard] Corruption detected! Restoring from in-memory backup.");
            IntegrityManager.Instance.NotifyPlayerOfDataRestoration();
            return lastKnownGoodSave;
        }

        // As a fallback, try to load from PlayerPrefs if the in-memory backup is gone
        string backupJson = PlayerPrefs.GetString(backupSaveSlot, null);
        if (!string.IsNullOrEmpty(backupJson))
        {
            Debug.LogWarning("[SaveIntegrityGuard] Corruption detected! Restoring from PlayerPrefs backup.");
            IntegrityManager.Instance.NotifyPlayerOfDataRestoration();
            return JsonUtility.FromJson<SaveData>(backupJson);
        }

        // If no backup exists, we must create a new save file.
        Debug.LogError("[SaveIntegrityGuard] Corruption detected, but no backup was found! Creating a new save file.");
        return new SaveData();
    }

    /// <summary>
    /// Validates the integrity of loaded save data.
    /// </summary>
    /// <param name="dataToValidate">The save data that was just loaded.</param>
    /// <returns>True if the data is valid, false otherwise.</returns>
    public bool ValidateSaveData(SaveData dataToValidate)
    { 
        // Example Rule 1: Currencies should not be negative.
        if (dataToValidate.playerMetaData.coins < 0 || dataToValidate.playerMetaData.gems < 0)
        {
            IntegrityManager.Instance.ReportError("Save data validation failed: Negative currency found.");
            return false;
        }

        // Example Rule 2: Player level should be within a reasonable range.
        // This range could be defined in a remote config.
        if (dataToValidate.playerMetaData.playerLevel > 200) // 200 is an arbitrary max level for this example
        {
            IntegrityManager.Instance.ReportError("Save data validation failed: Player level out of range.");
            return false;
        }

        // Add more validation rules here as needed.

        return true;
    }
}
