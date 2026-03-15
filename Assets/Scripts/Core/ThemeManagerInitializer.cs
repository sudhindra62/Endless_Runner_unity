using UnityEngine;

public class ThemeManagerInitializer : MonoBehaviour
{
    void Awake()
    {
        GameObject themeManagerGO = new GameObject("ThemeManager");
        ThemeManager themeManager = themeManagerGO.AddComponent<ThemeManager>();

        themeManager.themes = new ThemeSO[3];
        themeManager.themes[0] = Resources.Load<ThemeSO>("BaseCityTheme");
        themeManager.themes[1] = Resources.Load<ThemeSO>("ApocalypticCityTheme");
        themeManager.themes[2] = Resources.Load<ThemeSO>("CyberCityTheme");
    }
}
