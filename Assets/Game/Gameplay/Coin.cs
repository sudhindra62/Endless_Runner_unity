
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Gameplay
{
    public class Coin : MonoBehaviour
    {
        private void Start()
        {
            ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
            if (currentTheme != null && currentTheme.coinModelPrefab != null)
            {
                // Destroy the default model and instantiate the theme-specific one
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                Instantiate(currentTheme.coinModelPrefab, transform);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Add to player's coin count
                Destroy(gameObject);
            }
        }
    }
}
