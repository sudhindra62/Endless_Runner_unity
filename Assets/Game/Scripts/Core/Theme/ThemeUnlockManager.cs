using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Utils;

public class ThemeUnlockManager : MonoBehaviour
{
    public static ThemeUnlockManager Instance;

    private const string UNLOCKED_THEMES_KEY = "UnlockedThemes";

    private HashSet<string> unlockedThemes = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUnlockedThemes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsThemeUnlocked(ThemeConfig themeConfig)
    {
        if (themeConfig.unlockType == ThemeUnlockType.Free || unlockedThemes.Contains(themeConfig.name))
        {
            return true;
        }
        return false;
    }

    public bool UnlockThemeWithGems(ThemeConfig themeConfig)
    {
        if (themeConfig.unlockType == ThemeUnlockType.GemUnlock)
        {
            if (CurrencyManager.Instance.SpendGems(themeConfig.gemPrice))
            {
                UnlockTheme(themeConfig);
                return true;
            }
        }
        return false;
    }

    public bool UnlockThemeWithDiscount(ThemeConfig themeConfig, int discountPercentage)
    {
        if (themeConfig.unlockType == ThemeUnlockType.GemUnlock)
        {
            int discountedPrice = themeConfig.gemPrice - (themeConfig.gemPrice * discountPercentage / 100);
            if (CurrencyManager.Instance.SpendGems(discountedPrice))
            {
                UnlockTheme(themeConfig);
                return true;
            }
        }
        return false;
    }

    public void UnlockThemeForPremium()
    {
        ThemeConfig[] allThemes = Resources.LoadAll<ThemeConfig>("Themes");
        foreach (ThemeConfig theme in allThemes)
        {
            if (theme.unlockType == ThemeUnlockType.Premium)
            {
                UnlockTheme(theme);
            }
        }
    }

    private void UnlockTheme(ThemeConfig themeConfig)
    {
        unlockedThemes.Add(themeConfig.name);
        SaveUnlockedThemes();
    }

    private void LoadUnlockedThemes()
    {
        string unlockedThemesString = PlayerPrefs.GetString(UNLOCKED_THEMES_KEY, "");
        if (!string.IsNullOrEmpty(unlockedThemesString))
        {
            unlockedThemes = new HashSet<string>(unlockedThemesString.Split(','));
        }
    }

    private void SaveUnlockedThemes()
    {
        string unlockedThemesString = string.Join(",", unlockedThemes);
        PlayerPrefs.SetString(UNLOCKED_THEMES_KEY, unlockedThemesString);
        PlayerPrefs.Save();
    }
}
