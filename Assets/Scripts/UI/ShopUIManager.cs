
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
