
using System.Collections.Generic;

/// <summary>
/// Defines the comprehensive data structure for a player's entire game progress.
/// Expanded and fortified by Supreme Guardian Architect v12 to ensure 100% feature persistence.
/// This data contract is the single source of truth for all savable player data.
/// </summary>
[System.Serializable]
public class SaveData
{
    // --- CORE PROGRESSION ---
    public int HighScore;
    public string PlayerName;
    public bool TutorialCompleted;
    public long LastLoginTimestamp;
    public int TotalRuns;

    // --- CURRENCY & ECONOMY ---
    public int PrimaryCurrency; // e.g., Coins
    public int PremiumCurrency; // e.g., Gems

    // --- CHARACTER & CUSTOMIZATION ---
    public string EquippedCharacterId;
    public List<string> UnlockedCharacterSkins;

    // --- INVENTORY ---
    public Dictionary<string, int> PowerUpInventory; // Key: PowerUpID, Value: Quantity

    // --- BATTLE PASS & MISSIONS ---
    public int BattlePassTier;
    public int BattlePassXP;
    public List<string> ActiveMissionIDs;
    public Dictionary<string, int> MissionProgress; // Key: MissionID, Value: Current progress

    // --- GAME SETTINGS ---
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;
    public bool HapticsEnabled;

    // --- METADATA & INTEGRITY ---
    public string SaveVersion;
    public string AnalyticsChecksum; // A hash to verify the integrity of critical fields.
    public byte[] bestGhostRunData; // For ghost run feature
    public PlayerAchievementData achievementData;

    /// <summary>
    /// Initializes a new player's save file with default values.
    /// </summary>
    public SaveData()
    {
        // Core Progression
        HighScore = 0;
        PlayerName = "Player";
        TutorialCompleted = false;
        LastLoginTimestamp = 0;
        TotalRuns = 0;

        // Currency
        PrimaryCurrency = 0;
        PremiumCurrency = 0;

        // Character & Customization
        EquippedCharacterId = "default";
        UnlockedCharacterSkins = new List<string>();

        // Inventory
        PowerUpInventory = new Dictionary<string, int>();

        // Battle Pass & Missions
        BattlePassTier = 1;
        BattlePassXP = 0;
        ActiveMissionIDs = new List<string>();
        MissionProgress = new Dictionary<string, int>();

        // Game Settings
        MasterVolume = 1.0f;
        MusicVolume = 0.8f;
        SfxVolume = 1.0f;
        HapticsEnabled = true;

        // Metadata
        SaveVersion = "1.0";
        AnalyticsChecksum = "";
        bestGhostRunData = null;
        achievementData = new PlayerAchievementData();
    }
}
