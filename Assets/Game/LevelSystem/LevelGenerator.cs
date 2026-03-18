using UnityEngine;

namespace EndlessRunner.Level
{
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
                _themeManager = FindObjectOfType<ThemeManager>();
                if (_themeManager == null)
                {
                    Debug.LogError("ThemeManager not found. Please add a ThemeManager to the scene.");
                }
            }
        }

        public ThemeConfig GetCurrentTheme()
        {
            if (_themeManager != null)
            {
                return _themeManager.currentTheme;
            }
            Debug.LogWarning("ThemeManager not available, returning null theme.");
            return null;
        }
    }
}
