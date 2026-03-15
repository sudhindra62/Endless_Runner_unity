
using EndlessRunner.Themes;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class ThemeColorApplicator : MonoBehaviour
    {
        private void Start()
        {
            ThemeManager.Instance.OnThemeChanged += ApplyThemeColors;
            ApplyThemeColors(ThemeManager.Instance.GetCurrentTheme());
        }

        private void OnDestroy()
        {
            if (ThemeManager.Instance != null)
            {
                ThemeManager.Instance.OnThemeChanged -= ApplyThemeColors;
            }
        }

        private void ApplyThemeColors(ThemeData theme)
        {
            if (theme == null) return;

            // Find all UI elements that need their color changed
            Text[] textElements = FindObjectsOfType<Text>();
            foreach (Text text in textElements)
            {
                text.color = theme.uiAccentColor;
            }

            Image[] images = FindObjectsOfType<Image>();
            foreach (Image image in images)
            {
                // You might want to be more specific here, e.g., by using tags
                image.color = theme.uiAccentColor;
            }
        }
    }
}
