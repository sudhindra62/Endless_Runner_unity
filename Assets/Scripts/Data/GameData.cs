using System;
using System.Collections.Generic;


[Serializable]
public class GameData
{
    // Player Stats
    public int highScore;
    public float bestTime;
    public float bestDistance;
    public float totalDistance;
    public int totalCoins;
    public int gems;
    public int playerLevel;
    public float currentXP;
    public float currentRunXP; // Track XP earned in the current/last run to prevent duplicates
    public int pityCounter;
    public PlayerMetaData playerMetaData;
    public List<string> inventory;
    public List<string> inventoryItemIds;
    public List<string> unlockedCosmetics = new List<string>();
    public List<PlayerChestState> chestStates = new List<PlayerChestState>();
    public string activeCosmeticID;
    public List<string> unlockedSkins; // Legacy support
    public string activeTheme; // Legacy support
    public Dictionary<string, int> shardInventory;
    public List<string> unlockedCosmeticEffects;
    public Dictionary<string, int> fragmentInventory;
    public Dictionary<string, int> rareDropPityCounters;
    public List<string> processedTransactions;
    public bool isAdsRemoved;
    public bool isPremiumSubscribed;
    public long premiumExpirationTimestamp;
    public Dictionary<string, int> powerUpLevels;
    public long lastTimedChestClaimTimestamp;

    // Daily Reward
    public long lastDailyRewardTimestamp;
    public int dailyRewardStreak;
    public long lastLoginTimestamp;
    public int loginStreak;
    public Dictionary<string, bool> claimedDailyRewards;
    public long homeScreenLastRewardTimestamp;
    public List<string> unlockedThemes;
    public List<string> discountedThemes;
    public string tempThemeName;
    public long tempThemeUnlockTimestamp;
    public int tempThemeDurationHours;

    // Settings
    public bool hasCompletedTutorial;

    // Achievements
    public Dictionary<string, int> achievementProgress;
    public List<string> unlockedAchievements;
    public Dictionary<string, AchievementData> achievementData;
    
    // Missions & Challenges
    public Mission currentMission;
    public Dictionary<string, int> questProgress;
    public List<string> completedQuests;
    public string currentTrackedChallengeID;
    public int challengeAttemptsMade;
    public string lastChallengeAttemptDateId;

    // Ghost Runs
    public string ghostRunData;
    public string checksum;
    public GhostRunData bestGhostRunData;

    // Bonus Runs
    public int bonusRunsRemaining;
    public long nextBonusRunResetTimestamp;

    // Ad Session
    public int runsSinceLastInterstitial;
    public int adsWatchedToday;
    public int adSpinsUsedToday;
    public long lastAdResetTimestamp;

    // Battle Pass
    public int battlePassXP;
    public bool hasBattlePassPremium;
    public List<string> claimedBattlePassRewards;

    public GameData()
    {
        highScore = 0;
        bestTime = 0f;
        bestDistance = 0f;
        totalDistance = 0f;
        totalCoins = 0;
        gems = 0;
        playerLevel = 1;
        currentXP = 0f;
        playerMetaData = new PlayerMetaData();
        inventory = new List<string>();
        hasCompletedTutorial = false;
        achievementData = new Dictionary<string, AchievementData>();
        achievementProgress = new Dictionary<string, int>();
        unlockedAchievements = new List<string>();
        questProgress = new Dictionary<string, int>();
        completedQuests = new List<string>();
        battlePassXP = 0;
        hasBattlePassPremium = false;
        claimedBattlePassRewards = new List<string>();
        currentMission = null;
        lastDailyRewardTimestamp = 0;
        dailyRewardStreak = 0;
        lastLoginTimestamp = 0;
        loginStreak = 0;
        claimedDailyRewards = new Dictionary<string, bool>();
        unlockedThemes = new List<string>() { "Default" };
        discountedThemes = new List<string>();
        tempThemeName = "";
        tempThemeUnlockTimestamp = 0;
        tempThemeDurationHours = 0;
        inventoryItemIds = new List<string>();
        unlockedCosmetics = new List<string>();
        unlockedSkins = new List<string>();
        activeTheme = "Default";
        chestStates = new List<PlayerChestState>();
        shardInventory = new Dictionary<string, int>();
        unlockedCosmeticEffects = new List<string>();
        fragmentInventory = new Dictionary<string, int>();
        rareDropPityCounters = new Dictionary<string, int>();
        processedTransactions = new List<string>();
        isAdsRemoved = false;
        isPremiumSubscribed = false;
        premiumExpirationTimestamp = 0;
        activeCosmeticID = "";
        powerUpLevels = new Dictionary<string, int>();
        lastTimedChestClaimTimestamp = 0;
        adsWatchedToday = 0;
        adSpinsUsedToday = 0;
        lastAdResetTimestamp = 0;
        challengeAttemptsMade = 0;
        lastChallengeAttemptDateId = "";
        bonusRunsRemaining = 3;
        nextBonusRunResetTimestamp = 0;
    }

    // --- Shard Inventory Helpers for AEIS Folder 2 Sync ---
    public Dictionary<string, int> GetShardInventory() => shardInventory ?? (shardInventory = new Dictionary<string, int>());
    public void SetShardInventory(Dictionary<string, int> value) => shardInventory = value;
}

