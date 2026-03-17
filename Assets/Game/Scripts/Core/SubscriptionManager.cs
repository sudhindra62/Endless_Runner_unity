using UnityEngine;
using System;

public class SubscriptionManager : MonoBehaviour
{
    public static SubscriptionManager Instance;

    private const string PREMIUM_STATUS_KEY = "PremiumStatus";
    private const string PREMIUM_EXPIRATION_KEY = "PremiumExpiration";

    public bool IsPremium { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CheckSubscriptionStatus();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckSubscriptionStatus()
    {
        IsPremium = PlayerPrefs.GetInt(PREMIUM_STATUS_KEY, 0) == 1;
        if (IsPremium)
        {
            long expirationTicks = Convert.ToInt64(PlayerPrefs.GetString(PREMIUM_EXPIRATION_KEY));
            DateTime expirationTime = new DateTime(expirationTicks);
            if (DateTime.UtcNow > expirationTime)
            {
                IsPremium = false;
                PlayerPrefs.SetInt(PREMIUM_STATUS_KEY, 0);
                PlayerPrefs.Save();
            }
        }
    }

    public void Subscribe(int days)
    {
        IsPremium = true;
        DateTime expirationTime = DateTime.UtcNow.AddDays(days);
        PlayerPrefs.SetInt(PREMIUM_STATUS_KEY, 1);
        PlayerPrefs.SetString(PREMIUM_EXPIRATION_KEY, expirationTime.Ticks.ToString());
        PlayerPrefs.Save();

        ThemeUnlockManager.Instance.UnlockThemeForPremium();
    }
}
