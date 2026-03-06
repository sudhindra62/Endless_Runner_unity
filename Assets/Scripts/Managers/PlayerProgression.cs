
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

    private void Start()
    {
        // Load progression data from persistence
        // LoadProgression();
        CalculateXPForNextLevel();
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

        while (currentXP >= xpForNextLevel && xpForNextLevel > 0) // Add check to prevent infinite loop if curve is flat
        {
            LevelUp();
        }
        
        // SaveProgression();
    }

    private void LevelUp()
    {
        currentXP -= xpForNextLevel; // Subtract the threshold and carry over the rest
        currentLevel++;
        
        Debug.Log($"Leveled up to Level {currentLevel}!");
        OnLevelUp?.Invoke(currentLevel);

        // Check for Rank Up
        CheckForRankUp();
        
        // Calculate the requirement for the new level
        CalculateXPForNextLevel();
        
        // Grant level-up rewards via the central RewardManager
        // RewardManager.Instance.GrantLevelUpReward(currentLevel);
    }

    private void CheckForRankUp()
    {
        // Assumes ranks are sorted by levelRequired in the Inspector
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
        // Use the AnimationCurve to get the XP required for the *next* level.
        // The curve's 'time' is the current level, and 'value' is the XP needed.
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
        return currentRank; // Return lowest rank if none are met
    }

    // --- Persistence (Example) ---
    // private void SaveProgression() { PlayerPrefs.SetInt("PlayerLevel", currentLevel); ... }
    // private void LoadProgression() { currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1); ... }
}
