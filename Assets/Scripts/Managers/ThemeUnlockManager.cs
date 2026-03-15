
using EndlessRunner.Themes;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class ThemeUnlockManager : MonoBehaviour
    {
        public static ThemeUnlockManager Instance;

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

        public bool IsThemeUnlocked(ThemeSO theme)
        {
            return PlayerPrefs.GetInt(theme.themeName, 0) == 1;
        }

        public void UnlockTheme(ThemeSO theme)
        {
            PlayerPrefs.SetInt(theme.themeName, 1);
            PlayerPrefs.Save();
        }
    }
}
