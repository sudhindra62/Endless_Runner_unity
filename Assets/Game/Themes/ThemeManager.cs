
using UnityEngine;
using System;

namespace EndlessRunner.Themes
{
    public class ThemeManager : MonoBehaviour
    {
        public static ThemeManager Instance { get; private set; }

        public event Action<ThemeData> OnThemeChanged;

        [SerializeField]
        private ThemeData currentTheme;

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

        public void SetTheme(ThemeData newTheme)
        {
            currentTheme = newTheme;
            ApplyTheme();
            OnThemeChanged?.Invoke(currentTheme);
        }

        private void ApplyTheme()
        {
            if (currentTheme == null) return;

            // Apply Skybox
            if (currentTheme.skybox != null)
            {
                RenderSettings.skybox = currentTheme.skybox;
            }

            // Apply Lighting
            if (currentTheme.sun != null)
            {
                // You might want to add more sophisticated lighting controls here
            }

            // The rest of the theme application logic (e.g., for UI, music)
            // will be handled by other managers that will get the current theme from this ThemeManager.
        }

        public ThemeData GetCurrentTheme()
        {
            return currentTheme;
        }
    }
}
