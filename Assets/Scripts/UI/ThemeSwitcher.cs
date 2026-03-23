

using UnityEngine;

    public class ThemeSwitcher : MonoBehaviour
    {
        [SerializeField]
        private ThemeSO[] themes;
        private int currentThemeIndex = 0;

        private void Start()
        {
            if (themes.Length > 0)
            {
                ThemeManager.Instance.SetTheme(themes[0]);
            }
        }

        public void SwitchTheme()
        {
            if (themes.Length == 0) return;

            currentThemeIndex = (currentThemeIndex + 1) % themes.Length;
            ThemeManager.Instance.SetTheme(themes[currentThemeIndex]);
        }
    }

