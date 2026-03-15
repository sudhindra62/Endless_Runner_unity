
using EndlessRunner.Managers;
using EndlessRunner.Themes;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class ThemeShopItemUI : MonoBehaviour
    {
        public Text themeNameText;
        public Button unlockButton;
        public int unlockCost;

        private ThemeSO theme;

        public void Setup(ThemeSO themeSO)
        {
            theme = themeSO;
            themeNameText.text = theme.themeName;

            if (ThemeUnlockManager.Instance.IsThemeUnlocked(theme))
            {
                unlockButton.gameObject.SetActive(false);
            }
            else
            {
                unlockButton.onClick.AddListener(UnlockTheme);
            }
        }

        private void UnlockTheme()
        {    
            if(CurrencyManager.Instance.CurrentCoins >= unlockCost)
            {
                CurrencyManager.Instance.RemoveCoins(unlockCost);
                ThemeUnlockManager.Instance.UnlockTheme(theme);
                unlockButton.gameObject.SetActive(false);
            }
        }
    }
}
