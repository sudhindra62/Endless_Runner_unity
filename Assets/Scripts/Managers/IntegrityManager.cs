
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Monitors game state and player data for signs of cheating or tampering.
/// This system acts as a passive detection layer and flags suspicious sessions
/// without directly intervening or blocking the player.
/// </summary>
public class IntegrityManager : Singleton<IntegrityManager>
{
    // --- Public State ---
    public bool IsSessionSuspicious { get; private set; } = false;
    public HashSet<string> SuspensionReasons { get; } = new HashSet<string>();

    // --- Constants ---
    private const string LAST_TIMESTAMP_KEY = "last_session_timestamp_utc";
    private const double MAX_TIME_ROLLBACK_HOURS = -2.0;
    private const int MAX_SCORE_PER_SECOND = 10000;
    private const int MAX_COINS_PER_RUN = 50000;
    private const int MAX_RARE_DROPS_PER_HOUR = 10;
    private const float MIN_SECONDS_BETWEEN_RUNS = 5.0f;
    private const int MAX_RAPID_RESTARTS = 3;
    private const int MAX_XP_GAIN_PER_EVENT = 10000;

    // --- Private State for Validation ---
    private DateTime lastScoreValidationTimestamp;
    private float lastRunStartTime = -100f;
    private int rapidRestartCount = 0;
    private int lastKnownScore;
    private int lastKnownCoins;
    private int lastKnownGems;
    private int lastKnownXP;
    private int coinsEarnedThisRun;
    private readonly List<DateTime> rareDropTimestamps = new List<DateTime>();

    public void Initialize()
    {
        Debug.Log("IntegrityManager Initializing...");
        ValidateTime();
        SubscribeToGameEvents();

        lastKnownCoins = DataManager.Instance.Coins;
        lastKnownGems = DataManager.Instance.Gems;
        lastKnownScore = ScoreManager.Instance.Score;
        lastKnownXP = XPManager.Instance.CurrentXP;
    }

    private void OnDisable()
    {
        DataManager.OnCoinsChanged -= OnCoinsChanged;
        DataManager.OnGemsChanged -= OnGemsChanged;
        XPManager.OnXPChanged -= OnXPChanged;
        ScoreManager.OnScoreChanged -= OnScoreChanged;
        GameFlowController.OnRunStart -= OnRunStart;
        ReviveManager.OnReviveUsed -= OnReviveUsed;
        SkillTreeManager.OnSkillTreeChanged -= OnSkillTreeStateChanged;
        RareDropManager.OnRareDropAwarded -= OnRareDropAwarded;
    }

    private void SubscribeToGameEvents()
    {
        DataManager.OnCoinsChanged += OnCoinsChanged;
        DataManager.OnGemsChanged += OnGemsChanged;
        XPManager.OnXPChanged += OnXPChanged;
        ScoreManager.OnScoreChanged += OnScoreChanged;
        GameFlowController.OnRunStart += OnRunStart;
        ReviveManager.OnReviveUsed += OnReviveUsed;
        SkillTreeManager.OnSkillTreeChanged += OnSkillTreeStateChanged;
        RareDropManager.OnRareDropAwarded += OnRareDropAwarded;
    }

    private void ValidateTime()
    {
        string storedTicksStr = PlayerPrefs.GetString(LAST_TIMESTAMP_KEY, null);
        DateTime now = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(storedTicksStr) && long.TryParse(storedTicksStr, out long storedTicks))
        {
            DateTime lastSessionTime = new DateTime(storedTicks);
            TimeSpan delta = now - lastSessionTime;

            if (delta.TotalHours < MAX_TIME_ROLLBACK_HOURS)
            {
                FlagSuspicious("TimeManipulation_BackwardJump", $"Time jumped backward by {delta.TotalHours:F1} hours.");
            }
        }

        PlayerPrefs.SetString(LAST_TIMESTAMP_KEY, now.Ticks.ToString());
    }

    private void OnRunStart()
    {
        if (Time.time - lastRunStartTime < MIN_SECONDS_BETWEEN_RUNS)
        {
            rapidRestartCount++;
            if (rapidRestartCount > MAX_RAPID_RESTARTS)
            {
                FlagSuspicious("RareDropFarming_RestartAbuse", $"Session restarted {rapidRestartCount} times in quick succession.");
            }
        }
        else
        {
            rapidRestartCount = 0;
        }
        
        coinsEarnedThisRun = 0;
        lastScoreValidationTimestamp = Time.time;
        lastRunStartTime = Time.time;
    }

    private void OnCoinsChanged(int newTotal)
    {
        if (newTotal < 0) FlagSuspicious("CurrencyViolation_NegativeBalance", "Coin balance became negative.");

        int increase = newTotal - lastKnownCoins;
        if (increase < -2000000000) FlagSuspicious("CurrencyViolation_Overflow", "Coin balance overflow detected.");

        if (increase > 0)
        {
            coinsEarnedThisRun += increase;
            if (coinsEarnedThisRun > MAX_COINS_PER_RUN) FlagSuspicious("CurrencyViolation_ImpossibleRunGain", $"Earned {coinsEarnedThisRun} coins in a single run.");
        }

        lastKnownCoins = newTotal;
    }

    private void OnGemsChanged(int newTotal)
    {
         if (newTotal < 0) FlagSuspicious("CurrencyViolation_NegativeBalance", "Gem balance became negative.");

        int increase = newTotal - lastKnownGems;
        if (increase < -2000000000) FlagSuspicious("CurrencyViolation_Overflow", "Gem balance overflow detected.");

        if (increase > 5000) FlagSuspicious("CurrencyViolation_LargeGemJump", $"Gems increased by {increase} unexpectedly.");
        lastKnownGems = newTotal;
    }

    private void OnXPChanged(int newXP, int xpForNextLevel)
    {
        int increase = newXP - lastKnownXP;
        if (increase > MAX_XP_GAIN_PER_EVENT)
        {
            FlagSuspicious("XPViolation_ImpossibleGain", $"XP increased by {increase} in a single event.");
        }
        lastKnownXP = newXP;
    }

    private void OnScoreChanged(int newTotal)
    {
        float timeSinceLastCheck = Time.time - lastScoreValidationTimestamp;
        if (timeSinceLastCheck > 0)
        {
            int scoreIncrease = newTotal - lastKnownScore;
            float scoreRate = scoreIncrease / timeSinceLastCheck;

            if (scoreRate > MAX_SCORE_PER_SECOND) FlagSuspicious("ScoreViolation_ImpossibleRate", $"Score increased at {scoreRate:F0}/s (max: {MAX_SCORE_PER_SECOND}).");
        }
        lastKnownScore = newTotal;
        lastScoreValidationTimestamp = Time.time;
    }
    
    private void OnReviveUsed(int reviveCountInRun)
    {
        if (reviveCountInRun > ReviveManager.MAX_REVIVES_PER_RUN) FlagSuspicious("ReviveAbuse_ExcessiveRevives", $"Player used revive {reviveCountInRun} times.");
    }

    private void OnRareDropAwarded()
    {
        DateTime now = DateTime.UtcNow;
        rareDropTimestamps.Add(now);
        rareDropTimestamps.RemoveAll(timestamp => (now - timestamp).TotalHours > 1);

        if (rareDropTimestamps.Count > MAX_RARE_DROPS_PER_HOUR) FlagSuspicious("RareDropFarming_ExcessiveFrequency", $"Received {rareDropTimestamps.Count} rare drops in the last hour.");
    }

    private void OnSkillTreeStateChanged()
    {
        var skillTree = SkillTreeManager.Instance;
        if (skillTree.SkillPoints < 0) FlagSuspicious("SkillTreeViolation_NegativePoints", "Skill points went into the negative.");

        int totalSpentPoints = skillTree.SkillNodeLevels.Sum(node => node.Value);
        int totalEarnedPoints = skillTree.SkillPoints + totalSpentPoints;
        int expectedMaxPoints = PlayerMetaData.Instance.PlayerLevel - 1;

        if (totalEarnedPoints > expectedMaxPoints)
        {
            FlagSuspicious("SkillTreeViolation_ExcessivePoints", $"Player has {totalEarnedPoints} total skill points at level {PlayerMetaData.Instance.PlayerLevel} (expected max {expectedMaxPoints}).");
        }
    }

    public string CalculateHash(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private void FlagSuspicious(string reasonCode, string debugMessage)
    {
        if (SuspensionReasons.Contains(reasonCode)) return;

        IsSessionSuspicious = true;
        SuspensionReasons.Add(reasonCode);
        
        Debug.LogWarning($"INTEGRITY ALERT: {reasonCode}. {debugMessage}");
        
        // --- RESPONSE STRATEGY ---
        LeagueManager.Instance.DisableSubmissionsForSession();
    }
}
