
using UnityEngine;
using System;
using TMPro;

[Serializable]
public struct PlayerRank
{
    public string rankName;
    public int levelRequired;
    public Sprite rankIcon;
}

/// <summary>
/// The supreme singleton for managing player experience (XP), levels, ranks, and skill points.
/// This script now integrates with MilestoneManager to check for milestone completions.
/// </summary>
public class PlayerProgression : Singleton<PlayerProgression>
{
    [Header("XP & Leveling")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private long currentXP = 0;
    [SerializeField] private AnimationCurve xpToNextLevelCurve;

    [Header("Ranks")]
    [SerializeField] private PlayerRank[] playerRanks;

    [Header("UI Hooks")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private UnityEngine.UI.Image xpBar;

    public static event Action<int> OnLevelUp;
    public static event Action<long, long> OnXPChanged;
    public static event Action<long> OnXPGained;
    public static event Action<PlayerRank> OnRankUp;

    private long xpForNextLevel;
    private const string CurrentLevelKey = "PlayerLevel";
    private const string CurrentXPKey = "PlayerXP";
    private const string LastRunXpKey = "LastRunXp";

    protected override void Awake()
    {
        base.Awake();
        // Regression Check: Single Source of Truth [R32]
        // Progression is now loaded from SaveManager.Instance.Data
    }

    private void Start()
    {
        LoadProgression(); // Moved to Start to ensure SaveManager is ready
        CalculateXPForNextLevel();
        UpdateUI();
        if (SaveManager.Instance != null) SaveManager.Instance.Data.currentRunXP = 0;
    }

    /// <summary>
    /// Adds XP to the player's total, checks for level-ups, and updates milestones.
    /// </summary>
    public void AddXP(long amount, string source = "General")
    {
        if (amount <= 0) return;

        if (source == "RunComplete")
        {
            if (SaveManager.Instance != null && SaveManager.Instance.Data.currentRunXP > 0)
            {
                Debug.LogWarning("Duplicate XP addition from run detected. Ignoring.");
                return;
            }
            if (SaveManager.Instance != null) SaveManager.Instance.Data.currentRunXP = amount;
        }

        currentXP += amount;

        // ◈ ARCHITECT_OMEGA INTEGRATION: Check for XP-based milestones
        if (MilestoneManager.Instance != null)
        {
            MilestoneManager.Instance.CheckMilestones(MilestoneType.TotalXP, currentXP);
        }

        OnXPChanged?.Invoke(currentXP, xpForNextLevel);
        OnXPGained?.Invoke(currentXP);

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

        // ◈ ARCHITECT_OMEGA INTEGRATION: Check for level-based milestones
        if (MilestoneManager.Instance != null)
        {
            MilestoneManager.Instance.CheckMilestones(MilestoneType.PlayerLevel, currentLevel);
        }

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
                // ◈ ARCHITECT_OMEGA INTEGRATION: Check for rank-based milestones
                if (MilestoneManager.Instance != null)
                {
                    // Assuming rankName is unique. A better approach would be a RankID.
                    MilestoneManager.Instance.CheckMilestones(MilestoneType.Rank, currentLevel); 
                }
                break; 
            }
        }
    }
    
    private void CalculateXPForNextLevel()
    {
        if (xpToNextLevelCurve != null && xpToNextLevelCurve.length > 0)
        {
            xpForNextLevel = (long)xpToNextLevelCurve.Evaluate(currentLevel);
        }
        else
        {
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
    
    private void SaveProgression()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.Data.playerLevel = currentLevel;
        SaveManager.Instance.Data.currentXP = (float)currentXP;
        SaveManager.Instance.SaveGame();
    }

    private void LoadProgression()
    {
        if (SaveManager.Instance == null) return;
        currentLevel = SaveManager.Instance.Data.playerLevel;
        currentXP = (long)SaveManager.Instance.Data.currentXP;
    }

    [ContextMenu("Reset Player Progression")]
    public void ResetProgression()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Data.playerLevel = 1;
            SaveManager.Instance.Data.currentXP = 0;
            SaveManager.Instance.Data.currentRunXP = 0;
            SaveManager.Instance.SaveGame();
        }
        currentLevel = 1;
        currentXP = 0;
        CalculateXPForNextLevel();
        OnXPChanged?.Invoke(currentXP, xpForNextLevel);
        OnXPGained?.Invoke(currentXP);
        UpdateUI();
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
