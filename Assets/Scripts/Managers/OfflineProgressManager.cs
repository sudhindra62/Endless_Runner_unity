
using UnityEngine;
using System;

/// <summary>
/// Calculates and awards progress made while the player was offline.
/// </summary>
public class OfflineProgressManager : MonoBehaviour
{
    private const string LastSessionKey = "LastSessionTimestamp";
    private const int CoinsPerMinute = 1;

    /// <summary>
    /// Calculates and applies any offline progress.
    /// </summary>
    public void ApplyOfflineProgress()
    {
        DateTime lastSession = GetLastSessionTimestamp();
        DateTime now = DateTime.UtcNow;
        TimeSpan offlineDuration = now - lastSession;

        if (offlineDuration.TotalMinutes > 1)
        {
            int coinsEarned = (int)(offlineDuration.TotalMinutes * CoinsPerMinute);

            CurrencyManager currencyManager = ServiceLocator.Get<CurrencyManager>();
            if (currencyManager != null)
            {
                currencyManager.AddCoins(coinsEarned);
            }

            Debug.Log($"Welcome back! You earned {coinsEarned} coins while you were away.");
        }

        // Save the current time as the last session timestamp
        PlayerPrefs.SetString(LastSessionKey, now.ToString());
        PlayerPrefs.Save();
    }

    private DateTime GetLastSessionTimestamp()
    {
        string timestampString = PlayerPrefs.GetString(LastSessionKey, null);
        if (string.IsNullOrEmpty(timestampString))
        {
            return DateTime.UtcNow;
        }

        return DateTime.Parse(timestampString);
    }
}
