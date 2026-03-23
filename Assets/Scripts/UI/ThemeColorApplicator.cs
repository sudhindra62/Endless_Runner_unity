

using UnityEngine;
using UnityEngine.UI;

    public class ThemeColorApplicator : MonoBehaviour
    {
        private void Start()
        {
            ThemeManager.OnThemeChanged += ApplyThemeColors;
            ApplyThemeColors(ThemeManager.Instance != null ? ThemeManager.Instance.GetCurrentTheme() : null);
        }

        private void OnDestroy()
        {
            ThemeManager.OnThemeChanged -= ApplyThemeColors;
        }

        private void ApplyThemeColors(ThemeSO theme)
        {
            if (theme == null) return;

            // Find all UI elements that need their color changed
            Text[] textElements = FindObjectsByType<Text>(FindObjectsSortMode.None);
            foreach (Text text in textElements)
            {
                text.color = theme.uiAccentColor;
            }

            Image[] images = FindObjectsByType<Image>(FindObjectsSortMode.None);
            foreach (Image image in images)
            {
                // You might want to be more specific here, e.g., by using tags
                image.color = theme.uiAccentColor;
            }
        }
    }

