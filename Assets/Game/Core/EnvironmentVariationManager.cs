
using EndlessRunner.Themes;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class EnvironmentVariationManager : MonoBehaviour
    {
        public static EnvironmentVariationManager Instance { get; private set; }

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

        public void SpawnEnvironmentVariation()
        {
            ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
            if (currentTheme != null && currentTheme.environmentPrefabs.Length > 0)
            {
                int variationIndex = Random.Range(0, currentTheme.environmentPrefabs.Length);
                Instantiate(currentTheme.environmentPrefabs[variationIndex]);
            }
        }
    }
}
