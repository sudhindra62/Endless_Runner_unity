
using UnityEngine;
using System;

/// <summary>
/// Manages the player's progression, including Level and XP.
/// This is a persistent singleton that saves/loads data using PlayerPrefs.
/// It uses static events to notify other systems of changes (e.g., UI).
/// </summary>
public class PlayerProgression : MonoBehaviour
{
    public static PlayerProgression Instance;

    // --- Events ---
    public static event Action<int> OnLevelUp; // Fires when the player gains a new level.
    public static event Action<int, int, int> OnXPChanged; // Fires when XP changes. <currentXP, xpForThisLevel, xpForNextLevel>

    [Header("XP Configuration")]
    [Tooltip("How much XP is needed for the first level.")]
    [SerializeField] private int baseXpRequirement = 100;
    [Tooltip("How much the XP requirement increases with each level.")]
    [SerializeField] private float xpMultiplierPerLevel = 1.5f;
    [Tooltip("XP awarded per point of score at the end of a run.")]
    [SerializeField] private float xpPerScorePoint = 0.1f;

    // --- Public Properties ---
    public int CurrentLevel { get; private set; }
    public int CurrentXP { get; private set; }
    public int XpForNextLevel { get; private set; }

    // --- PlayerPrefs Keys ---
    private const string LevelKey = "PlayerProgression_Level";
    private const string XPKey = "PlayerProgression_XP";

    #region Unity Lifecycle Methods

    void Awake()
    {
        // --- Singleton Pattern ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgression();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Trigger initial UI update
        OnXPChanged?.Invoke(CurrentXP, 0, XpForNextLevel);
    }

    #endregion

    #region Public API

    /// <summary>
    /// Grants XP to the player, typically called at the end of a run.
    /// Checks for level-ups automatically.
    /// </summary>
    /// <param name="scoreFromRun">The final score achieved in the run.</param>
    public void GrantXPForRun(int scoreFromRun)
    {
        if (scoreFromRun <= 0) return;

        int xpGained = Mathf.FloorToInt(scoreFromRun * xpPerScorePoint);
        CurrentXP += xpGained;
        Debug.Log($"Granted {xpGained} XP for a score of {scoreFromRun}.");

        CheckForLevelUp();
        SaveProgression();
        OnXPChanged?.Invoke(CurrentXP, GetXPForLevel(CurrentLevel), XpForNextLevel);
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Checks if the current XP is enough to level up, and handles the level-up logic if so.
    /// </summary>
    private void CheckForLevelUp()
    {
        while (CurrentXP >= XpForNextLevel)
        {
            CurrentXP -= XpForNextLevel;
            CurrentLevel++;
            UpdateXpRequirement();
            
            Debug.Log($"Player leveled up to Level {CurrentLevel}!");
            OnLevelUp?.Invoke(CurrentLevel);
        }
    }

    /// <summary>
    /// Calculates the XP required for the next level.
    /// </summary>
    private void UpdateXpRequirement()
    {
        XpForNextLevel = GetXPForLevel(CurrentLevel + 1);
    }
    
    /// <summary>
    /// Gets the total XP required to reach a specific level.
    /// </summary>
    public int GetXPForLevel(int level)
    {
        if (level <= 1) return baseXpRequirement;
        return (int)(baseXpRequirement * Mathf.Pow(level, xpMultiplierPerLevel));
    }

    /// <summary>
    /// Saves the player's current level and XP to PlayerPrefs.
    /// </summary>
    private void SaveProgression()
    {
        PlayerPrefs.SetInt(LevelKey, CurrentLevel);
        PlayerPrefs.SetInt(XPKey, CurrentXP);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the player's level and XP from PlayerPrefs.
    /// </summary>
    private void LoadProgression()
    {
        CurrentLevel = PlayerPrefs.GetInt(LevelKey, 1);
        CurrentXP = PlayerPrefs.GetInt(XPKey, 0);
        UpdateXpRequirement();
    }

    #endregion
}
