
using EndlessRunner.Themes;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class ThemeManager : MonoBehaviour
    {
        public static ThemeManager Instance { get; private set; }

        public List<ThemeSO> themes;
        private int currentThemeIndex;

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

            // Load the saved theme index, or default to 0
            currentThemeIndex = PlayerPrefs.GetInt("CurrentTheme", 0);
        }

        public void SetTheme(int themeIndex)
        {
            if (themeIndex >= 0 && themeIndex < themes.Count)
            {
                currentThemeIndex = themeIndex;
                PlayerPrefs.SetInt("CurrentTheme", currentThemeIndex);
                PlayerPrefs.Save();
            }
        }

        public ThemeSO GetCurrentTheme()
        {
            if (themes.Count == 0) return null;
            return themes[currentThemeIndex];
        }

        public ThemeSO[] GetThemes()
        {
            return themes.ToArray();
        }
    }
}
