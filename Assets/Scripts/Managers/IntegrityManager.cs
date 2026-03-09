
using UnityEngine;

/// <summary>
/// Manages the integrity of the save data, including validation and backup/restore.
/// Created by Supreme Guardian Architect v12 to fulfill the A-to-Z Connectivity mandate for SaveManager.
/// </summary>
public class IntegrityManager : Singleton<IntegrityManager>
{
    public SaveIntegrityGuard saveIntegrityGuard { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        saveIntegrityGuard = new SaveIntegrityGuard();
    }

    public void ReportError(string errorMessage)
    {
        // In a real game, this might send an error report to a developer service.
        Debug.LogWarning($"[IntegrityManager] An integrity issue was reported: {errorMessage}");
    }
}

/// <summary>
/// A class dedicated to validating and managing backups of the SaveData.
/// </summary>
public class SaveIntegrityGuard
{
    private SaveData _backupData;

    public void CreateBackup(SaveData currentData)
    {
        // Create a deep copy of the save data to ensure the backup is independent.
        _backupData = new SaveData
        {
            HighScore = currentData.HighScore,
            TotalCurrency = currentData.TotalCurrency,
            TutorialCompleted = currentData.TutorialCompleted,
            UnlockedCharacterIDs = new System.Collections.Generic.List<string>(currentData.UnlockedCharacterIDs)
        };
        Debug.Log("[IntegrityGuard] Created a backup of the current save data.");
    }

    public SaveData RestoreBackup()
    {
        Debug.LogWarning("[IntegrityGuard] Restoring save data from backup.");
        if (_backupData == null)
        {
            Debug.LogError("[IntegrityGuard] No backup available to restore. Returning a new default SaveData.");
            return new SaveData();
        }
        return _backupData;
    }

    public bool ValidateSaveData(SaveData dataToValidate)
    {
        // Basic validation: ensure the object is not null and perform some simple checks.
        if (dataToValidate == null) return false;

        // Example validation: scores and currency should not be negative.
        if (dataToValidate.HighScore < 0 || dataToValidate.TotalCurrency < 0)
        {
            Debug.LogWarning("[IntegrityGuard] Validation failed: Negative high score or currency detected.");
            return false;
        }

        // Add more complex validation logic here as needed (e.g., checksums).

        return true;
    }
}
