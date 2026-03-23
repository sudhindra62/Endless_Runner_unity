
using System;
using UnityEngine;

    /// <summary>
    /// Manages all In-App Purchase logic.
    /// Provides a simplified interface for purchasing items like coins or shards.
    /// </summary>
    public class IAPManager : Singleton<IAPManager>
    {
        // Placeholder for a real IAP SDK client (e.g., Unity IAP)
        private object iapClient;
        private bool isIapInitialized = false;

        public event Action<string, bool> OnPurchaseCompleted;

        public void InitializeIAP()
        {
            if (isIapInitialized) return;

            // In a real scenario, this would involve a complex, asynchronous initialization
            // with the app store to retrieve product information.
            Debug.Log("IAP_MANAGER: IAP SDK Initializing...");
            isIapInitialized = true;
            Debug.Log("IAP_MANAGER: IAP SDK Initialized successfully.");
        }

        /// <summary>
        /// Initiates a purchase for a given product ID.
        /// </summary>
        /// <param name="productId">The unique identifier for the product (e.g., "com.endlessrunner.1000coins").</param>
        public void InitiatePurchase(string productId)
        {
            if (!isIapInitialized)
            {
                Debug.LogError("IAP_MANAGER: Cannot initiate purchase, IAP is not initialized.");
                OnPurchaseCompleted?.Invoke(productId, false);
                return;
            }

            Debug.Log($"IAP_MANAGER: Purchase initiated for product: {productId}");

            // --- Simulation of a successful purchase ---
            // In a real app, this would be a callback from the store (e.g., ProcessPurchase)
            ProcessSuccessfulPurchase(productId);
        }

        public void ConfirmPurchase(string productId) => InitiatePurchase(productId);

        /// <summary>
        /// This method would be called by the IAP SDK upon a successful transaction.
        /// </summary>
        private void ProcessSuccessfulPurchase(string productId)
        {
            Debug.Log($"IAP_MANAGER: Purchase successful for product: {productId}");

            // Grant the item to the player based on the product ID
            switch (productId)
            {
                case "com.endlessrunner.1000coins":
                    DataManager.Instance.GameData.totalCoins += 1000;
                    break;
                case "com.endlessrunner.5000coins":
                    DataManager.Instance.GameData.totalCoins += 5000;
                    break;
                case "com.endlessrunner.10rareshards":
                    var inventory = DataManager.Instance.GameData.GetShardInventory();
                    string rareKey = ShardType.Rare.ToString();
                    if (!inventory.ContainsKey(rareKey))
                    {
                        inventory[rareKey] = 0;
                    }
                    inventory[rareKey] += 10;
                    DataManager.Instance.GameData.SetShardInventory(inventory);
                    break;
                default:
                    Debug.LogWarning($"IAP_MANAGER: Unknown product ID purchased: {productId}");
                    break;
            }
            
            // Save the changes and notify listeners
            DataManager.Instance.SaveGameData();
            OnPurchaseCompleted?.Invoke(productId, true);
        }

        /// <summary>
        // This method would be called by the IAP SDK upon a failed transaction.
        /// </summary>
        private void ProcessFailedPurchase(string productId, string reason)
        {
            Debug.LogError($"IAP_MANAGER: Purchase failed for product: {productId}. Reason: {reason}");
            OnPurchaseCompleted?.Invoke(productId, false);
        }
    }

