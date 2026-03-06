
using System;
using UnityEngine;

/// <summary>
/// Calculates and grants rewards to the player based on the time they have been away from the game.
/// </summary>
public class OfflineProgressManager : Singleton<OfflineProgressManager>
{
    [Header("Configuration")]
    [SerializeField] private int coinsPerMinute = 1;
    [SerializeField] private int maxOfflineMinutes = 240; // Max time to accumulate rewards (e.g., 4 hours)

    private const string QUIT_TIME_KEY = "AppQuitTime";

    private void Start()
    {
        CheckForOfflineProgress();
    }

    private void CheckForOfflineProgress()
    {
        if (!PlayerPrefs.HasKey(QUIT_TIME_KEY))
        {
            return; // No stored quit time, so can't calculate progress.
        }

        long temp = Convert.ToInt64(PlayerPrefs.GetString(QUIT_TIME_KEY));
        DateTime quitTime = DateTime.FromBinary(temp);
        DateTime now = DateTime.UtcNow;

        TimeSpan offlineDuration = now - quitTime;

        if (offlineDuration.TotalMinutes > 1) // Only grant if offline for more than a minute
        {
            int minutesOffline = (int)Math.Min(offlineDuration.TotalMinutes, maxOfflineMinutes);
            
            int coinsToGrant = minutesOffline * coinsPerMinute;

            if (coinsToGrant > 0)
            {
                // Grant the currency via a centralized manager
                // CurrencyManager.Instance.AddCoins(coinsToGrant);

                Debug.Log($"Player was offline for {minutesOffline} minutes. Granted {coinsToGrant} coins.");
                
                // UIManager.Instance.ShowOfflineProgressPopup(minutesOffline, coinsToGrant);
            }
        }

        // Clear the quit time key so this logic doesn't run again until the next session.
        PlayerPrefs.DeleteKey(QUIT_TIME_KEY);
    }

    private void OnApplicationQuit()
    {
        // Save the current time when the application quits.
        PlayerPrefs.SetString(QUIT_TIME_KEY, DateTime.UtcNow.ToBinary().ToString());
        PlayerPrefs.Save();
        Debug.Log("Saving quit time for offline progress calculation.");
    }
}
