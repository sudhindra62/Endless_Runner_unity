
using System;
using UnityEngine;

/// <summary>
/// Manages the player's high score, ensuring it is persisted through the central SaveManager.
/// Architecturally rewritten by Supreme Guardian Architect v12 to enforce a single source of truth for all game data.
/// This system is now fully integrated with the project's persistence layer.
/// </summary>
public class HighScoreManager : Singleton<HighScoreManager>
{
    // --- EVENTS ---
    public static event Action<int> OnHighScoreChanged;

    // --- PUBLIC PROPERTIES ---
    public int HighScore { get; private set; }

    // --- UNITY LIFECYCLE & SAVE SYSTEM INTEGRATION ---

    protected override void Awake()
    {
        base.Awake();
        HighScore = 0; // Initialize with a default value.
    }

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to the central persistence system. ---
        SaveManager.OnLoad += HandleLoad;
        SaveManager.OnBeforeSave += HandleBeforeSave;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks. ---
        SaveManager.OnLoad -= HandleLoad;
        SaveManager.OnBeforeSave -= HandleBeforeSave;
    }

    private void HandleLoad()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            HandleLoad(SaveManager.Instance.Data);
        }
    }

    /// <summary>
    /// Populates the HighScoreManager's state from the master save data.
    /// </summary>
    private void HandleLoad(GameData data)
    {
        HighScore = data.highScore;
        Debug.Log($"Guardian Architect: High Score loaded: {HighScore}");
        OnHighScoreChanged?.Invoke(HighScore);
    }

    private void HandleBeforeSave()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            HandleBeforeSave(SaveManager.Instance.Data);
        }
    }

    /// <summary>
    /// Populates the master save data with the HighScoreManager's current state before saving.
    /// </summary>
    private void HandleBeforeSave(GameData data)
    {
        data.highScore = HighScore;
    }

    // --- PUBLIC API ---

    /// <summary>
    /// Reports a new score. If it's higher than the current high score, it will be updated.
    /// </summary>
    /// <param name="newScore">The score from the completed game run.</param>
    /// <returns>True if a new high score was set, false otherwise.</returns>
    public bool ReportScore(int newScore)
    {
        if (newScore > HighScore)
        {
            Debug.Log($"Guardian Architect: New High Score achieved! Old: {HighScore}, New: {newScore}");
            HighScore = newScore;
            OnHighScoreChanged?.Invoke(HighScore);
            return true;
        }
        return false;
    }

    public static int GetHighScore()
    {
        return Instance != null ? Instance.HighScore : 0;
    }
}
