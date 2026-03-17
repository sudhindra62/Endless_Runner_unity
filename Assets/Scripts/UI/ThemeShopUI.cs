
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Themes;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    public class ThemeShopUI : MonoBehaviour
    {
        public GameObject themeShopPanel;
        public Transform themeContainer;
        public GameObject themeButtonPrefab;

        public void OpenShop()
        {
            themeShopPanel.SetActive(true);
            PopulateShop();
        }

        public void CloseShop()
        {
            themeShopPanel.SetActive(false);
        }

        private void PopulateShop()
        {
            foreach (Transform child in themeContainer)
            {
                Destroy(child.gameObject);
            }

            ThemeSO[] themes = ThemeManager.Instance.GetAllThemes();
            foreach (ThemeSO theme in themes)
            {
                GameObject buttonGO = Instantiate(themeButtonPrefab, themeContainer);
                ThemeShopItemUI itemUI = buttonGO.GetComponent<ThemeShopItemUI>();
                itemUI.Setup(theme);
            }
        }
    }
}
