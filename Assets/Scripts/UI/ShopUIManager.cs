
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Managers;
using System.Collections.Generic;

namespace EndlessRunner.UI
{
    public class ShopUIManager : MonoBehaviour
    {
        public GameObject ShopItemPrefab;
        public Transform ShopItemContainer;

        private void Start()
        {
            InitializeShopUI();
        }

        private void InitializeShopUI()
        {
            List<ShopItem> shopItems = ShopManager.Instance.ShopItems;

            foreach (ShopItem item in shopItems)
            {
                GameObject shopItemGO = Instantiate(ShopItemPrefab, ShopItemContainer);
                shopItemGO.GetComponent<ShopItemUI>().Initialize(item);
            }
        }
    }
}
