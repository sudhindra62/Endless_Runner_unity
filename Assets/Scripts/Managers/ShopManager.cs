
using UnityEngine;
using System.Collections.Generic;

    public class ShopManager : Singleton<ShopManager>
    {
        public List<ShopItem> ShopItems { get; private set; }
        public static event System.Action<string> OnSkinPurchased;

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
                    itemId = itemData.itemId,
                    itemName = itemData.itemName,
                    description = itemData.description,
                    cost = itemData.cost,
                    type = itemData.type,
                    icon = itemData.icon
                });
            }
        }

        public bool PurchaseItem(ShopItem item)
        {
            if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.SpendCurrency(CurrencyType.Coins, item.Price))
            {
                Debug.Log($"Guardian Architect: Purchased {item.Name} for {item.Price} coins.");
                PlayerDataManager.Instance.AddItem(item.itemId);
                OnSkinPurchased?.Invoke(item.itemId);
                return true;
            }
            return false;
        }

        public void PurchaseProduct(string productId)
        {
            ShopItem item = ShopItems.Find(i => i.itemId == productId);
            if (item != null) PurchaseItem(item);
        }

        public List<ProductData> GetProductsByCategory(string category)
        {
            if (!System.Enum.TryParse(category, out ProductCategory parsedCategory))
            {
                return new List<ProductData>();
            }

            return GetProductsByCategory(parsedCategory);
        }

        public List<ProductData> GetProductsByCategory(ProductCategory category)
        {
            ProductData[] products = Resources.LoadAll<ProductData>("Shop");
            List<ProductData> matches = new List<ProductData>();

            foreach (ProductData product in products)
            {
                if (product != null && product.Category == category)
                {
                    matches.Add(product);
                }
            }

            return matches;
        }
    }

