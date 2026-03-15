
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Gameplay
{
    public class EnemyChaser : MonoBehaviour
    {
        private void Start()
        {
            ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
            if (currentTheme != null && currentTheme.enemyChaserPrefab != null)
            {
                // Destroy the default model and instantiate the theme-specific one
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                Instantiate(currentTheme.enemyChaserPrefab, transform);
            }
        }

        // Add chasing logic here
    }
}
