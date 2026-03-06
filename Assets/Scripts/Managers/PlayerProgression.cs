
using UnityEngine;
using System;
using TMPro; // Required for UI elements

// Defines a rank based on player level
[Serializable]
public struct PlayerRank
{
    public string rankName;
    public int levelRequired;
    public Sprite rankIcon;
}

/// <summary>
/// The supreme singleton for managing player experience (XP), levels, ranks, and skill points.
/// This script has absorbed all functionality from the redundant XPManager.
/// </summary>
public class PlayerProgression : Singleton<PlayerProgression>
{
    [Header("XP & Leveling")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private long currentXP = 0;
    [SerializeField] private AnimationCurve xpToNextLevelCurve; // Defines XP needed for each level

    [Header("Ranks")]
    [SerializeField] private PlayerRank[] playerRanks;

    [Header("UI Hooks")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private UnityEngine.UI.Image xpBar;

    public static event Action<int> OnLevelUp;
    public static event Action<long, long> OnXPChanged; // ◈ MERGED: More descriptive XP event
    public static event Action<PlayerRank> OnRankUp;

    private long xpForNextLevel;
    private const string CurrentLevelKey = "PlayerLevel";
    private const string CurrentXPKey = "PlayerXP";
    private const string LastRunXpKey = "LastRunXp";

    protected override void Awake()
    {
        base.Awake();
        LoadProgression();
    }

    private void Start()
    {
        CalculateXPForNextLevel();
        UpdateUI();
        PlayerPrefs.DeleteKey(LastRunXpKey); // ◈ MERGED: Clear last run XP on start
    }

    /// <summary>
    /// Adds XP to the player's total and checks for level-ups.
    /// </summary>
    /// <param name="amount">The amount of XP to grant.</param>
    /// <param name="source">The source of the XP, used for duplicate checks.</param>
    public void AddXP(long amount, string source = "General") // ◈ MERGED: Source parameter added
    {
        if (amount <= 0) return;

        // ◈ MERGED: Safeguard against duplicate XP from a single run.
        if (source == "RunComplete")
        {
            if (PlayerPrefs.HasKey(LastRunXpKey))
            {
                Debug.LogWarning("Duplicate XP addition from run detected. Ignoring.");
                return;
            }
            PlayerPrefs.SetInt(LastRunXpKey, (int)amount);
        }

        currentXP += amount;
        OnXPChanged?.Invoke(currentXP, xpForNextLevel);

        while (currentXP >= xpForNextLevel && xpForNextLevel > 0)
        {
            LevelUp();
        }
        
        UpdateUI();
        SaveProgression();
    }

    private void LevelUp()
    {
        currentXP -= xpForNextLevel; 
        currentLevel++;
        
        Debug.Log($"Leveled up to Level {currentLevel}!");
        OnLevelUp?.Invoke(currentLevel);

        // ◈ MERGED: Grant Skill Point on level up
        if (SkillTreeManager.Instance != null)
        {
            SkillTreeManager.Instance.AddSkillPoints(1);
            Debug.Log("Granted 1 Skill Point.");
        }

        CheckForRankUp();
        CalculateXPForNextLevel();
        
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.GrantLevelUpReward(currentLevel);
        }

        UpdateUI();
    }

    private void CheckForRankUp()
    {
        foreach (var rank in playerRanks)
        {
            if (rank.levelRequired == currentLevel)
            {
                Debug.Log($"New Rank Achieved: {rank.rankName}!");
                OnRankUp?.Invoke(rank);
                break; 
            }
        }
    }
    
    private void CalculateXPForNextLevel()
    {
        // Ensure the curve has keys before evaluating
        if (xpToNextLevelCurve != null && xpToNextLevelCurve.length > 0)
        {
            xpForNextLevel = (long)xpToNextLevelCurve.Evaluate(currentLevel);
        }
        else
        {
            // If no curve, fallback to a default calculation to prevent division by zero
            xpForNextLevel = currentLevel * 100;
            Debug.LogWarning("XPToNextLevelCurve is not set. Using default calculation.");
        }
    }
    
    private void UpdateUI()
    {
        if (levelText != null) levelText.SetText($"LVL {currentLevel}");
        if (xpBar != null)
        {
            xpBar.fillAmount = (xpForNextLevel > 0) ? ((float)currentXP / xpForNextLevel) : 1f;
        }
    }
    
    // ◈ MERGED: PlayerPrefs saving from XPManager
    private void SaveProgression()
    {
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.SetString(CurrentXPKey, currentXP.ToString()); // Use string for long
        PlayerPrefs.Save();
    }

    // ◈ MERGED: PlayerPrefs loading from XPManager
    private void LoadProgression()
    {
        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        currentXP = long.Parse(PlayerPrefs.GetString(CurrentXPKey, "0"));
    }

    // ◈ MERGED: Debug tool for resetting progress
    [ContextMenu("Reset Player Progression")]
    public void ResetProgression()
    {
        PlayerPrefs.DeleteKey(CurrentLevelKey);
        PlayerPrefs.DeleteKey(CurrentXPKey);
        PlayerPrefs.DeleteKey(LastRunXpKey);
        currentLevel = 1;
        currentXP = 0;
        CalculateXPForNextLevel();
        OnXPChanged?.Invoke(currentXP, xpForNextLevel);
        UpdateUI();
        SaveProgression();
        Debug.Log("Player progression has been reset.");
    }

    public int GetCurrentLevel() => currentLevel;
    public long GetCurrentXP() => currentXP;
    public long GetXPForNextLevel() => xpForNextLevel;

    public PlayerRank GetCurrentRank()
    {
        PlayerRank currentRank = playerRanks.Length > 0 ? playerRanks[0] : new PlayerRank();
        for (int i = playerRanks.Length - 1; i >= 0; i--)
        {
            if (currentLevel >= playerRanks[i].levelRequired)
            {
                return playerRanks[i];
            }
        }
        return currentRank;
    }
}
