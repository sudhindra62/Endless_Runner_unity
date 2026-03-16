
using UnityEngine;
using System.Collections.Generic;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    public List<ThemeConfig> themes;
    public ThemeConfig currentTheme;

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
            RenderSettings.skybox = currentTheme.skyboxMaterial;
            RenderSettings.fogColor = currentTheme.fogColor;
            RenderSettings.fogDensity = currentTheme.fogDensity;
            DynamicGI.UpdateEnvironment();
        }
    }
}
