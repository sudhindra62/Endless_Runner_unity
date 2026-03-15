
using UnityEngine;
using EndlessRunner.Core;
using System.Collections.Generic;
using EndlessRunner.Data;

namespace EndlessRunner.Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        public List<ShopItem> ShopItems { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitializeShop();
        }

        private void InitializeShop()
        {
            ShopItems = new List<ShopItem>();
            ShopItemData[] shopItemData = Resources.LoadAll<ShopItemData>("ShopItems");

            foreach (ShopItemData itemData in shopItemData)
            {
                ShopItems.Add(new ShopItem
                {
                    Name = itemData.Name,
                    Description = itemData.Description,
                    Price = itemData.Price,
                    Type = itemData.Type
                });
            }
        }

        public bool PurchaseItem(ShopItem item)
        {
            if (GameManager.Instance.Coins >= item.Price)
            {
                GameManager.Instance.AddCoins(-item.Price);
                Debug.Log("Purchase successful!");
                // TODO: Add the item to the player's inventory
                return true;
            }
            return false;
        }
    }
}
