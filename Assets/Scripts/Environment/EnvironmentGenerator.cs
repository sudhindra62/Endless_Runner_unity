
using UnityEngine;

    public class EnvironmentGenerator : MonoBehaviour
    {
        public static EnvironmentGenerator Instance { get; private set; }

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

        public void Generate()
        {
            ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
            Debug.Log($"Generating environment with theme: {currentTheme.name}");
            // In a real implementation, you would use the theme's prefabs to build the level.
        }
    }
