
using EndlessRunner.Themes;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class ThemeManager : MonoBehaviour
    {
        public static ThemeManager Instance;

        public ThemeSO currentTheme;

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

        public void SetTheme(ThemeSO theme)
        {
            currentTheme = theme;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (currentTheme == null) return;

            RenderSettings.skybox.SetColor("_SkyTint", currentTheme.skyColor);
        }
    }
}
