
using UnityEngine;
using System;

// Defines a rank based on player level
[Serializable]
public struct PlayerRank
{
    public string rankName;
    public int levelRequired;
    public Sprite rankIcon;
}

/// <summary>
/// Manages player experience (XP), levels, and ranks.
/// Integrates with other systems to grant XP and trigger rewards on level-up.
/// Fulfills the 'Player Progression/XP' and 'Rank System' requirements.
/// </summary>
public class PlayerProgression : Singleton<PlayerProgression>
{
    [Header("XP & Leveling")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private long currentXP = 0;
    [SerializeField] private AnimationCurve xpToNextLevelCurve; // Defines XP needed for each level

    [Header("Ranks")]
    [SerializeField] private PlayerRank[] playerRanks;

    public static event Action<int> OnLevelUp;
    public static event Action<long> OnXPGained;
    public static event Action<PlayerRank> OnRankUp;

    private long xpForNextLevel;

    protected override void Awake()
    {
        base.Awake();
        LoadProgression();
    }

    private void Start()
    {
        GameManager.OnGameStart += HandleGameStart;
        CalculateXPForNextLevel();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= HandleGameStart;
    }

    private void HandleGameStart()
    {
        // Optional: Reset or apply any session-specific progression logic here
    }

    /// <summary>
    /// Adds XP to the player's total and checks for level-ups.
    /// </summary>
    /// <param name="amount">The amount of XP to grant.</param>
    public void AddXP(long amount)
    {
        if (amount <= 0) return;

        currentXP += amount;
        OnXPGained?.Invoke(currentXP);

        while (currentXP >= xpForNextLevel && xpForNextLevel > 0)
        {
            LevelUp();
        }
        
        SaveProgression();
    }

    private void LevelUp()
    {
        currentXP -= xpForNextLevel; 
        currentLevel++;
        
        Debug.Log($"Leveled up to Level {currentLevel}!");
        OnLevelUp?.Invoke(currentLevel);

        CheckForRankUp();
        
        CalculateXPForNextLevel();
        
        // Grant level-up rewards via the central RewardManager
        RewardManager.Instance.GrantLevelUpReward(currentLevel);
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
        xpForNextLevel = (long)xpToNextLevelCurve.Evaluate(currentLevel);
    }

    public int GetCurrentLevel() => currentLevel;
    public long GetCurrentXP() => currentXP;
    public long GetXPForNextLevel() => xpForNextLevel;

    public PlayerRank GetCurrentRank()
    {
        PlayerRank currentRank = playerRanks[0];
        for (int i = playerRanks.Length - 1; i >= 0; i--)
        {
            if (currentLevel >= playerRanks[i].levelRequired)
            {
                return playerRanks[i];
            }
        }
        return currentRank;
    }

    private void SaveProgression()
    {
        SaveData data = SaveSystem.LoadData() ?? new SaveData();
        data.currentLevel = this.currentLevel;
        data.currentXP = this.currentXP;
        SaveSystem.SaveData(data);
    }

    private void LoadProgression()
    {
        SaveData data = SaveSystem.LoadData();
        if (data != null)
        {
            this.currentLevel = data.currentLevel;
            this.currentXP = data.currentXP;
        }
    }
}
