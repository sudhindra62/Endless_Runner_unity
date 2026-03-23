

using UnityEngine;

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
            if (theme == null) return false;

            if (DailyRewardManager.Instance != null && DailyRewardManager.Instance.IsThemeTemporarilyUnlocked(theme)) 
            {
                return true;
            }

            switch (theme.unlockType)
            {
                case ThemeUnlockType.Free:
                    return true;
                case ThemeUnlockType.GemUnlock:
                case ThemeUnlockType.PremiumSubscription:
                    return (SaveManager.Instance != null && SaveManager.Instance.Data.unlockedThemes.Contains(theme.themeName)) 
                           || (theme.unlockType == ThemeUnlockType.PremiumSubscription && SubscriptionManager.Instance != null && SubscriptionManager.Instance.IsSubscribed());
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
                    if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.SpendCurrency(CurrencyType.Gems, theme.gemPrice))
                    {
                        MarkThemeAsUnlocked(theme);
                        return true;
                    }
                    return false;
                case ThemeUnlockType.PremiumSubscription:
                    if (SubscriptionManager.Instance != null && SubscriptionManager.Instance.IsSubscribed())
                    {
                        MarkThemeAsUnlocked(theme);
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public bool UnlockThemeWithDiscount(ThemeSO theme, int discountPercentage)
        {
            if (IsThemeUnlocked(theme)) return true;
            if (theme.unlockType != ThemeUnlockType.GemUnlock) return false;

            int discountedPrice = theme.gemPrice - (theme.gemPrice * discountPercentage / 100);
            if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.SpendCurrency(CurrencyType.Gems, discountedPrice))
            {
                MarkThemeAsUnlocked(theme);
                return true;
            }
            return false;
        }

        private void MarkThemeAsUnlocked(ThemeSO theme)
        {
            if (SaveManager.Instance != null && !SaveManager.Instance.Data.unlockedThemes.Contains(theme.themeName))
            {
                SaveManager.Instance.Data.unlockedThemes.Add(theme.themeName);
                SaveManager.Instance.SaveGame();
            }
        }

        public void UnlockAllPremiumThemes()
        {
            if (SaveManager.Instance == null) return;
            
            ThemeSO[] allThemes = Resources.LoadAll<ThemeSO>("Themes");
            foreach (ThemeSO theme in allThemes)
            {
                if (theme.unlockType == ThemeUnlockType.PremiumSubscription)
                {
                    if (!SaveManager.Instance.Data.unlockedThemes.Contains(theme.themeName))
                    {
                        SaveManager.Instance.Data.unlockedThemes.Add(theme.themeName);
                    }
                }
            }
            SaveManager.Instance.SaveGame();
        }
    }

