
using UnityEngine;
using System;

/// <summary>
/// A persistent singleton that stores all long-term player meta-data.
/// It uses PlayerPrefs for storage and provides safe, public methods for data manipulation.
/// Data is loaded on startup and saved automatically whenever a value is modified.
/// </summary>
public partial class PlayerMetaData : MonoBehaviour
{
    public static PlayerMetaData Instance { get; private set; }

    // --- PlayerPrefs Keys (Centralized) ---
    private const string TotalCoinsKey = "Meta_TotalCoins";
    private const string TotalGemsKey = "Meta_TotalGems";
    private const string TotalRunsKey = "Meta_TotalRuns";
    private const string TotalDistanceKey = "Meta_TotalDistance";
    private const string TotalPlayTimeKey = "Meta_TotalPlayTime";

    // --- Public Properties (Read-Only) ---
    public int TotalCoins { get; private set; }
    public int TotalGems { get; private set; }
    public int TotalRuns { get; private set; }
    public float TotalDistance { get; private set; }
    public float TotalPlayTime { get; private set; }

    // --- Events for other systems to listen to ---
    public static event Action<int> OnTotalCoinsChanged;
    public static event Action<int> OnTotalGemsChanged;

    #region Unity Lifecycle

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

    #endregion

    #region Public API

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        TotalCoins += amount;
        PlayerPrefs.SetInt(TotalCoinsKey, TotalCoins);
        PlayerPrefs.Save();
        OnTotalCoinsChanged?.Invoke(TotalCoins);
        // FUTURE HOOK: Check for achievements related to total coins earned.
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        TotalGems += amount;
        PlayerPrefs.SetInt(TotalGemsKey, TotalGems);
        PlayerPrefs.Save();
        OnTotalGemsChanged?.Invoke(TotalGems);
    }
    
    public void IncrementRuns()
    {
        TotalRuns++;
        PlayerPrefs.SetInt(TotalRunsKey, TotalRuns);
        PlayerPrefs.Save();
        // FUTURE HOOK: Check for achievements related to total runs completed.
    }

    public void AddDistance(float amount)
    {
        if (amount <= 0) return;
        TotalDistance += amount;
        PlayerPrefs.SetFloat(TotalDistanceKey, TotalDistance);
        PlayerPrefs.Save();
        // FUTURE HOOK: Check for daily challenges or missions related to distance.
    }
    
    public void AddPlayTime(float seconds)
    {
        if (seconds <= 0) return;
        TotalPlayTime += seconds;
        PlayerPrefs.SetFloat(TotalPlayTimeKey, TotalPlayTime);
        PlayerPrefs.Save();
    }

    #endregion

    #region Private Methods

    private void LoadData()
    {
        TotalCoins = PlayerPrefs.GetInt(TotalCoinsKey, 0);
        TotalGems = PlayerPrefs.GetInt(TotalGemsKey, 0);
        TotalRuns = PlayerPrefs.GetInt(TotalRunsKey, 0);
        TotalDistance = PlayerPrefs.GetFloat(TotalDistanceKey, 0f);
        TotalPlayTime = PlayerPrefs.GetFloat(TotalPlayTimeKey, 0f);
        Debug.Log("PlayerMetaData loaded.");
    }

    #endregion
}
