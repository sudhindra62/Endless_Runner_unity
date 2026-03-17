
using EndlessRunner.Themes;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class ThemeUnlockManager : MonoBehaviour
    {
        public static ThemeUnlockManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsThemeUnlocked(ThemeSO theme)
        {
            if (DailyRewardManager.Instance.IsThemeTemporarilyUnlocked(theme)) 
            {
                return true;
            }

            switch (theme.unlockType)
            {
                case ThemeUnlockType.Free:
                    return true;
                case ThemeUnlockType.GemUnlock:
                    return PlayerPrefs.GetInt(theme.themeName, 0) == 1;
                case ThemeUnlockType.PremiumSubscription:
                    return SubscriptionManager.Instance.IsSubscribed() || PlayerPrefs.GetInt(theme.themeName, 0) == 1;
                default:
                    return false;
            }
        }

        public bool UnlockTheme(ThemeSO theme)
        {
            if (IsThemeUnlocked(theme)) return true;

            switch (theme.unlockType)
            {
                case ThemeUnlockType.GemUnlock:
                    if (CurrencyManager.Instance.Gems >= theme.gemPrice)
                    {
                        CurrencyManager.Instance.SpendGems(theme.gemPrice);
                        PlayerPrefs.SetInt(theme.themeName, 1);
                        PlayerPrefs.Save();
                        return true;
                    }
                    return false;
                case ThemeUnlockType.PremiumSubscription:
                    if (SubscriptionManager.Instance.IsSubscribed())
                    {
                        PlayerPrefs.SetInt(theme.themeName, 1);
                        PlayerPrefs.Save();
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}
