using UnityEngine;
using System;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }
    public ThemeConfig[] themes;
    private int currentThemeIndex = -1;

    public static Action<ThemeConfig> OnThemeChanged;

    public ThemeConfig CurrentConfig { get; private set; }

    void Awake()
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

    void Start()
    {
        // Load the last selected theme or default to the first one
        int lastThemeIndex = PlayerPrefs.GetInt("SelectedTheme", 0);
        SetTheme(lastThemeIndex, true);
    }

    public void SetTheme(int themeIndex, bool force = false)
    {
        if (themeIndex >= 0 && themeIndex < themes.Length && (themeIndex != currentThemeIndex || force))
        {
            currentThemeIndex = themeIndex;
            CurrentConfig = themes[themeIndex];
            
            PlayerPrefs.SetInt("SelectedTheme", themeIndex);
            PlayerPrefs.Save();

            OnThemeChanged?.Invoke(CurrentConfig);
        }
    }

    public int GetCurrentThemeIndex()
    {
        return currentThemeIndex;
    }

    public int GetTotalThemes()
    {
        return themes.Length;
    }

    public Sprite GetCurrentThemeIcon()
    {
        if (CurrentConfig != null)
        {
            return CurrentConfig.themeIcon;
        }
        return null;
    }
}
