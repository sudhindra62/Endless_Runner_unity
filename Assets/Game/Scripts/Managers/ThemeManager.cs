using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Environment;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    public List<ThemeConfig> themes;
    public ThemeConfig currentTheme;
    public Lighting lighting;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadThemes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadThemes()
    {
        themes = new List<ThemeConfig>(Resources.LoadAll<ThemeConfig>("Themes"));
        if (themes.Count > 0)
        {
            currentTheme = themes[0];
            ApplyTheme();
        }
    }

    public void SetTheme(ThemeConfig theme)
    {
        currentTheme = theme;
        ApplyTheme();
    }

    void ApplyTheme()
    {
        if (currentTheme != null)
        {
            if (lighting != null)
            {
                lighting.ApplyTheme(currentTheme);
            }
            DynamicGI.UpdateEnvironment();
        }
    }
}
