using UnityEngine;
using System.Collections.Generic;

// I am assuming the ThemeManager is in the EndlessRunner.Themes namespace.
// Please adjust if this is not correct.
using EndlessRunner.Themes;

namespace EndlessRunner.Level
{
    /// <summary>
    /// The LevelGenerator is the master controller for the procedural level generation system.
    /// It is responsible for managing the theme and providing it to the SpawnController.
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        public static LevelGenerator Instance { get; private set; }

        [Header("Theme Management")]
        [SerializeField] private ThemeManager _themeManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (_themeManager == null)
            {
                // Find the ThemeManager in the scene if it hasn't been assigned.
                _themeManager = FindObjectOfType<ThemeManager>();
                if (_themeManager == null)
                {
                    Debug.LogError("ThemeManager not found in the scene. Please add a ThemeManager to the scene and assign it to the LevelGenerator.");
                }
            }
        }

        /// <summary>
        /// Returns the current theme from the ThemeManager.
        /// </summary>
        /// <returns>The current ThemeConfig.</returns>
        public ThemeConfig GetCurrentTheme()
        {
            if (_themeManager != null)
            {
                return _themeManager.currentTheme;
            }
            return null;
        }
    }
}
