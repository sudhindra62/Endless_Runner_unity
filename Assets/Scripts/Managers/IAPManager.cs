
using System;
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// Manages all In-App Purchases. Provides a framework for purchasing virtual goods.
    /// This is a placeholder for a real IAP SDK integration (e.g., Unity IAP).
    /// </summary>
    public class IAPManager : Singleton<IAPManager>
    {
        public event Action<string> OnPurchaseSuccess;
        public event Action<string> OnPurchaseFailure;

        // Example Product IDs
        public const string ProductIdCoinsTier1 = "com.omni_guardian.endlessrunner.coins1";
        public const string ProductIdCoinsTier2 = "com.omni_guardian.endlessrunner.coins2";
        public const string ProductIdRemoveAds = "com.omni_guardian.endlessrunner.removeads";

        public void InitializeIAP()
        {
            Debug.Log("IAP_MANAGER: Initializing In-App Purchase systems...");
            // In a real implementation, you would initialize your IAP SDK here.
        }

        /// <summary>
        /// Initiates a purchase for the given product ID.
        /// </summary>
        public void PurchaseProduct(string productId)
        {
            Debug.Log($"IAP_MANAGER: Attempting to purchase product: {productId}");
            // Simulate a delay for the purchase flow
            Invoke(nameof(SimulatePurchaseSuccess), 2.0f);
        }

        private void SimulatePurchaseSuccess(string productId)
        {
            Debug.Log($"IAP_MANAGER: Purchase successful for product: {productId}");
            OnPurchaseSuccess?.Invoke(productId);

            // Example of how to handle the reward
            if (DataManager.Instance != null)
            {
                int coinsToAdd = 0;
                switch (productId)
                {
                    case ProductIdCoinsTier1:
                        coinsToAdd = 1000;
                        break;
                    case ProductIdCoinsTier2:
                        coinsToAdd = 5000;
                        break;
                    case ProductIdRemoveAds:
                        // Set a flag in GameData to disable ads
                        Debug.Log("IAP_MANAGER: Ads have been permanently removed.");
                        break;
                }

                if (coinsToAdd > 0)
                {
                    GameManager.Instance.AddCoins(coinsToAdd);
                    DataManager.Instance.GameData.totalCoins = GameManager.Instance.Coins;
                    DataManager.Instance.SaveData();
                }
            }
        }
    }
}
