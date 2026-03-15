
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    public class ThemeShopUI : MonoBehaviour
    {
        public GameObject shopItemPrefab;
        public Transform shopItemContainer;

        void Start()
        {
            PopulateShop();
        }

        void PopulateShop()
        {
            foreach (Transform child in shopItemContainer)
            {
                Destroy(child.gameObject);
            }

            List<Themes.ThemeUnlockData> themes = ThemeManager.Instance.GetThemes();

            foreach (var themeData in themes)
            {
                GameObject shopItemGO = Instantiate(shopItemPrefab, shopItemContainer);
                ThemeShopItemUI itemUI = shopItemGO.GetComponent<ThemeShopItemUI>();
                itemUI.Setup(themeData);
            }
        }
    }
}
