
using System.Collections.Generic;

/// <summary>
/// The master data structure for all persistent game state.
/// This class is serialized to a file by the SaveManager.
/// Fortified by Supreme Guardian Architect v13 to ensure data integrity.
/// </summary>
[System.Serializable]
public class SaveData
{
    // --- CORE PROGRESSION ---
    public int HighScore;
    public bool TutorialCompleted;

    // --- ECONOMY ---
    public int PrimaryCurrency;
    public int PremiumCurrency;
    public bool AdsRemoved; // Added by Guardian Architect for IAPManager integration

    // --- CUSTOMIZATION & UNLOCKS ---
    public List<string> UnlockedCharacterIDs;

    /// <summary>
    /// Constructor to initialize default values for a new player.
    /// </summary>
    public SaveData()
    {
        HighScore = 0;
        TutorialCompleted = false;
        PrimaryCurrency = 0;
        PremiumCurrency = 0;
        AdsRemoved = false;
        UnlockedCharacterIDs = new List<string>();
    }
}
