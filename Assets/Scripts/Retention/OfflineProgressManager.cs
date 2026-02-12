
using UnityEngine;
using System;

/// <summary>
/// Calculates and grants a passive coin reward based on the time the player was offline.
/// This script is a persistent singleton.
/// 
/// --- How It Works ---
/// On game start (Awake), it compares the current time with the last saved time (OnApplicationQuit).
/// It calculates the duration spent offline and grants a reward based on a configurable rate,
/// up to a maximum cap.
/// </summary>
public class OfflineProgressManager : MonoBehaviour
{
    public static OfflineProgressManager Instance;

    [Header("Offline Reward Configuration")]
    [Tooltip("Coins granted per hour spent offline.")]
    [SerializeField] private int coinsPerHour = 50;
    
    [Tooltip("The maximum number of coins that can be earned while offline.")]
    [SerializeField] private int maxOfflineCoinCap = 1000;
    
    [Tooltip("The minimum offline duration (in seconds) to trigger a reward.")]
    [SerializeField] private int minDurationForReward = 300; // 5 minutes

    // --- PlayerPrefs Key ---
    private const string LastSessionTimeKey = "OfflineProgress_LastSessionTime";

    public static event Action<int> OnOfflineRewardGranted;

    void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CalculateAndGrantOfflineProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        // Save the exact time the game is closed
        PlayerPrefs.SetString(LastSessionTimeKey, DateTime.UtcNow.ToBinary().ToString());
        PlayerPrefs.Save();
        Debug.Log("OfflineProgressManager: Saved session end time.");
    }

    /// <summary>
    /// Calculates offline time and grants rewards if applicable.
    /// </summary>
    private void CalculateAndGrantOfflineProgress()
    {
        string lastTimeStr = PlayerPrefs.GetString(LastSessionTimeKey, "");
        if (string.IsNullOrEmpty(lastTimeStr)) return;

        long lastTimeBinary = Convert.ToInt64(lastTimeStr);
        DateTime lastSessionTime = DateTime.FromBinary(lastTimeBinary);
        
        TimeSpan offlineDuration = DateTime.UtcNow - lastSessionTime;

        Debug.Log($"Offline for: {offlineDuration.TotalMinutes:F1} minutes.");

        if (offlineDuration.TotalSeconds < minDurationForReward)
        {
            Debug.Log("Offline duration too short, no reward granted.");
            return;
        }

        // Calculate reward
        double hoursOffline = offlineDuration.TotalHours;
        int coinsEarned = Mathf.FloorToInt((float)hoursOffline * coinsPerHour);
        coinsEarned = Mathf.Min(coinsEarned, maxOfflineCoinCap); // Enforce cap

        if (coinsEarned > 0)
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCoins(coinsEarned);
                OnOfflineRewardGranted?.Invoke(coinsEarned);
                Debug.Log($"Granted {coinsEarned} offline coins!");
            }
            else
            {
                Debug.LogWarning("OfflineProgressManager: CurrencyManager not found. Cannot grant offline coins.");
            }
        }
        
        // Clear the key so this reward is only granted once per offline session
        PlayerPrefs.DeleteKey(LastSessionTimeKey);
    }
}
