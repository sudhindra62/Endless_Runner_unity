
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Managers;
using EndlessRunner.Themes;

namespace EndlessRunner.UI
{
    public class ThemeShopButton : MonoBehaviour
    {
        public Text themeNameText;
        public Image themePreviewImage;
        public Button purchaseButton;
        public Button playButton;
        public Text priceText;

        private ThemeSO theme;

        public void Setup(ThemeSO theme)
        {
            this.theme = theme;
            themeNameText.text = theme.themeName;
            // You would set the themePreviewImage.sprite to a preview image for the theme here

            ThemeUnlockData unlockData = ThemeUnlockManager.Instance.themeUnlockData.Find(t => t.theme == theme);

            if (ThemeUnlockManager.Instance.IsThemeUnlocked(theme))
            {
                playButton.gameObject.SetActive(true);
                purchaseButton.gameObject.SetActive(false);
            }
            else
            {
                playButton.gameObject.SetActive(false);
                purchaseButton.gameObject.SetActive(true);
                priceText.text = unlockData.unlockType == UnlockType.Gems ? unlockData.gemCost.ToString() + " GEMS" : "PREMIUM";
            }

            purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPurchaseButtonClicked()
        {
            ThemeUnlockData unlockData = ThemeUnlockManager.Instance.themeUnlockData.Find(t => t.theme == theme);
            if (unlockData.unlockType == UnlockType.Gems)
            {
                ThemeUnlockManager.Instance.UnlockThemeWithGems(theme);
            }
            else if (unlockData.unlockType == UnlockType.Premium)
            {
                ThemeUnlockManager.Instance.UnlockThemeWithPremium(theme);
            }

            // Refresh the button state after purchase attempt
            Setup(theme);
        }

        private void OnPlayButtonClicked()
        {
            ThemeManager.Instance.SetTheme(ThemeManager.Instance.themes.IndexOf(theme));
            // You would likely transition to the game scene here
        }
    }
}
