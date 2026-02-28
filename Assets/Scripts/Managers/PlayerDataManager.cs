
using UnityEngine;
using System;

/// <summary>
/// A centralized, persistent manager for all player-related data, including currency and progression.
/// It handles loading from and saving to PlayerPrefs, providing a single source of truth for player stats.
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    // --- Player Data ---
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public int Level { get; private set; }
    public int XP { get; private set; }
    public int XpForNextLevel { get; private set; }

    [Header("XP Configuration")]
    [Tooltip("The base XP required for the first level-up.")]
    [SerializeField] private int baseXpRequirement = 100;
    [Tooltip("The multiplier that increases the XP requirement per level.")]
    [SerializeField] private float xpMultiplierPerLevel = 1.5f;
    [Tooltip("The conversion rate from score to XP at the end of a run.")]
    [SerializeField] private float xpPerScorePoint = 0.1f;

    // --- Events ---
    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;
    public static event Action<int> OnLevelChanged;
    public static event Action<int, int> OnXPChanged;

    #region Unity Lifecycle & Persistence

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    #endregion

    #region Currency Management

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        Gems += amount;
        OnGemsChanged?.Invoke(Gems);
    }

    public bool SpendGems(int amount)
    {
        if (amount <= 0 || Gems < amount) return false;
        Gems -= amount;
        OnGemsChanged?.Invoke(Gems);
        return true;
    }

    #endregion

    #region Progression Management

    public void AddXPFromRun(int scoreFromRun)
    {
        if (scoreFromRun <= 0) return;

        int xpGained = Mathf.FloorToInt(scoreFromRun * xpPerScorePoint);
        if (xpGained <= 0) return;

        XP += xpGained;
        Debug.Log($"Granted {xpGained} XP for a score of {scoreFromRun}.");

        CheckForLevelUp();
        OnXPChanged?.Invoke(XP, XpForNextLevel);
    }

    private void CheckForLevelUp()
    {
        while (XP >= XpForNextLevel)
        {
            XP -= XpForNextLevel;
            Level++;
            UpdateXpRequirement();
            
            Debug.Log($"Player leveled up to Level {Level}!");
            OnLevelChanged?.Invoke(Level);
        }
    }

    private void UpdateXpRequirement()
    {
        XpForNextLevel = GetXPForLevel(Level + 1);
    }

    private int GetXPForLevel(int level)
    {
        if (level <= 1) return baseXpRequirement;
        return (int)(baseXpRequirement * Mathf.Pow(level, xpMultiplierPerLevel));
    }

    #endregion

    #region Data Persistence Implementation

    private void LoadData()
    {
        Coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        Gems = PlayerPrefs.GetInt("PlayerGems", 0);
        Level = PlayerPrefs.GetInt("PlayerLevel", 1);
        XP = PlayerPrefs.GetInt("PlayerXP", 0);
        UpdateXpRequirement();

        // Invoke events to ensure the UI is updated on load
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
        OnLevelChanged?.Invoke(Level);
        OnXPChanged?.Invoke(XP, XpForNextLevel);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("PlayerCoins", Coins);
        PlayerPrefs.SetInt("PlayerGems", Gems);
        PlayerPrefs.SetInt("PlayerLevel", Level);
        PlayerPrefs.SetInt("PlayerXP", XP);
        PlayerPrefs.Save();
        Debug.Log("Player data saved successfully.");
    }

    #endregion
}
