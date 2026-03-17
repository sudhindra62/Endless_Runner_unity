
using UnityEngine;

public class ThemeSelector : MonoBehaviour
{
    public ThemeDatabase themeDatabase;
    public string selectedThemeName;

    void Start()
    {
        if (themeDatabase != null && themeDatabase.themes != null && themeDatabase.themes.Length > 0)
        {
            ThemeConfig selectedTheme = null;
            if (!string.IsNullOrEmpty(selectedThemeName))
            {
                foreach (ThemeConfig theme in themeDatabase.themes)
                {
                    if (theme.themeName == selectedThemeName)
                    {
                        selectedTheme = theme;
                        break;
                    }
                }
            }

            if (selectedTheme == null)
            {
                int randomIndex = Random.Range(0, themeDatabase.themes.Length);
                selectedTheme = themeDatabase.themes[randomIndex];
            }

            if (selectedTheme != null)
            {
                Instantiate(selectedTheme.themeManagerPrefab);
            }
        }
    }
}
