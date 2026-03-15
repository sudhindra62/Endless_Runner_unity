
using EndlessRunner.Themes;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class ThemeShop : MonoBehaviour
    {
        public GameObject themeShopItemPrefab;
        public Transform shopItemContainer;
        public ThemeSO[] themes;

        private void Start()
        {
            foreach (var theme in themes)
            {
                GameObject shopItem = Instantiate(themeShopItemPrefab, shopItemContainer);
                ThemeShopItemUI itemUI = shopItem.GetComponent<ThemeShopItemUI>();
                itemUI.Setup(theme);
            }
        }
    }

}
