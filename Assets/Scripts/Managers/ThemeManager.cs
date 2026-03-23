using UnityEngine;
using System;
using System.Collections.Generic;



    /// <summary>
    /// The authoritative manager for all environment themes.
    /// Handles theme switching, visual application, and provides theme-specific prefabs.
    /// </summary>
    public class ThemeManager : Singleton<ThemeManager>
    {
        public ThemeSO[] themes;
        private int currentThemeIndex = -1;
        
        public static event Action<ThemeSO> OnThemeChanged;
        public static event Action<ThemeSO> OnThemeUnlocked;
        public ThemeSO CurrentConfig { get; private set; }
        public ThemeSO CurrentTheme => CurrentConfig;
        public ThemeSO currentTheme { get => CurrentConfig; set => SetTheme(value); }

        protected override void Awake()
        {
            base.Awake();
            if (themes == null || themes.Length == 0)
            {
                LoadThemesFromResources();
            }
        }

        private void Start()
        {
            if (SaveManager.Instance == null) return;
            string lastThemeName = SaveManager.Instance.Data.activeTheme;
            int lastThemeIndex = Array.FindIndex(themes, t => t.themeName == lastThemeName);
            if (lastThemeIndex == -1) lastThemeIndex = 0;
            SetTheme(lastThemeIndex, true);
        }

        private void LoadThemesFromResources()
        {
            ThemeSO[] loadedThemes = Resources.LoadAll<ThemeSO>("Themes");
            if (loadedThemes != null && loadedThemes.Length > 0)
                themes = loadedThemes;
        }

        public void SetTheme(int themeIndex, bool force = false)
        {
            if (themes != null && themeIndex >= 0 && themeIndex < themes.Length && (themeIndex != currentThemeIndex || force))
            {
                currentThemeIndex = themeIndex;
                CurrentConfig = themes[themeIndex];
                
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.Data.activeTheme = CurrentConfig.themeName;
                    SaveManager.Instance.SaveGame();
                }

                ApplyThemeVisuals();
                OnThemeChanged?.Invoke(CurrentConfig);
            }
        }

        public void SetTheme(ThemeSO theme)
        {
            if (themes == null) return;
            for (int i = 0; i < themes.Length; i++)
            {
                if (themes[i] == theme) { SetTheme(i); break; }
            }
        }

        private void ApplyThemeVisuals()
        {
            if (CurrentConfig != null)
            {
                if (CurrentConfig.skybox != null) RenderSettings.skybox = CurrentConfig.skybox;
                RenderSettings.fogColor = CurrentConfig.fogColor;
                RenderSettings.fogDensity = CurrentConfig.fogDensity;
                
                if (CurrentConfig.sunLight != null) CurrentConfig.sunLight.gameObject.SetActive(true);
                if (CurrentConfig.moonLight != null) CurrentConfig.moonLight.gameObject.SetActive(false);
            }
        }

        // --- Compatibility Getters ---

        public GameObject GetCoinPrefab() => CurrentConfig != null ? CurrentConfig.coinPrefab : null;
        public GameObject GetEnemyChaserPrefab() => CurrentConfig != null ? CurrentConfig.enemyChaserPrefab : null;
        public GameObject[] GetEnvironmentModules() => CurrentConfig != null ? CurrentConfig.environmentModules : null;
        public ThemeSO[] GetAllThemes() => themes;
        public Sprite GetCurrentThemeIcon() => CurrentConfig != null ? CurrentConfig.themeIcon : null;
        public int GetCurrentThemeIndex() => currentThemeIndex;
        public int GetTotalThemes() => themes != null ? themes.Length : 0;
        
        // Obsolete but kept for backward compatibility during transition
        public ThemeSO GetCurrentTheme() => CurrentConfig;

        public void UnlockTheme(ThemeSO theme)
        {
            // Placeholder for theme unlock logic
            if (theme != null)
            {
                SetTheme(theme);
                OnThemeUnlocked?.Invoke(theme);
            }
        }

        public ThemeProgress GetThemeProgress()
        {
            return new ThemeProgress { currentThemeIndex = currentThemeIndex };
        }

        public ThemeSO GetThemeByID(string themeID)
        {
            if (themes == null) return null;
            return System.Array.Find(themes, t => t.themeName == themeID);
        }

        public GameObject SpawnThemeEnvironment(Vector3 position)
        {
            if (CurrentConfig?.environmentModules == null || CurrentConfig.environmentModules.Length == 0) return null;
            var prefab = CurrentConfig.environmentModules[0];
            return Instantiate(prefab, position, Quaternion.identity);
        }

        public GameObject[] GetThemeAssets() => CurrentConfig?.environmentModules ?? new GameObject[0];

        public void ApplyThemeVisuals(ThemeSO theme)
        {
            if (theme != null) SetTheme(theme);
        }

        // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

        public void SetTheme(string themeID)
        {
            var theme = GetThemeByID(themeID);
            if (theme != null) SetTheme(theme);
        }

        public void SetTheme(int themeIndex)
        {
            if (themes != null && themeIndex >= 0 && themeIndex < themes.Length)
            {
                SetTheme(themes[themeIndex]);
            }
        }

        public ThemeSO GetThemeByIndex(int index)
        {
            return (themes != null && index >= 0 && index < themes.Length) ? themes[index] : null;
        }

        public int GetThemeIndexByID(string themeID)
        {
            return System.Array.FindIndex(themes, t => t.themeName == themeID);
        }

        public void UnlockTheme(string themeID)
        {
            var theme = GetThemeByID(themeID);
            if (theme != null) UnlockTheme(theme);
        }
    }


