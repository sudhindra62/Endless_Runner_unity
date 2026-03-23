
using System.Collections.Generic;
using UnityEngine;

public class EntitlementResolver : Singleton<EntitlementResolver>
{
    private const string RemoveAdsKey = "IAP_RemoveAds";
    private const string ProcessedTransactionsKey = "ProcessedTransactions";

    private List<string> _processedTransactionIds = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        _processedTransactionIds = SaveManager.Instance != null ? SaveManager.Instance.Data.processedTransactions : new List<string>();
    }

    public void ResolvePurchase(string productId, string transactionId)
    {
        if (IsTransactionProcessed(transactionId))
        {
            Debug.LogWarning($"Transaction '{transactionId}' has already been processed. Ignoring.");
            IAPManager.Instance.ConfirmPurchase(productId);
            return;
        }

        bool success = false;
        switch (productId)
        {
            case "com.gamestudio.remove_ads":
                GrantRemoveAds();
                success = true;
                break;
            case "com.gamestudio.revive_tokens_5":
                // Assume a ReviveTokenManager exists
                // ReviveTokenManager.Instance.AddTokens(5);
                success = true;
                break;
            case "com.gamestudio.gem_pack_1":
                DataManager.Instance.AddGems(100);
                success = true;
                break;
            case "com.gamestudio.coin_pack_1":
                DataManager.Instance.AddCoins(5000);
                success = true;
                break;
            case "com.gamestudio.premium_subscription":
                // Grant subscription benefits
                success = true;
                break;
        }

        if (success)
        {
            AddProcessedTransaction(transactionId);
            IAPManager.Instance.ConfirmPurchase(productId);
        }
    }

    private void GrantRemoveAds()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.Data.isAdsRemoved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Ads have been permanently removed.");
    }

    public bool AreAdsRemoved()
    {
        return SaveManager.Instance != null && SaveManager.Instance.Data.isAdsRemoved;
    }

    private void AddProcessedTransaction(string transactionId)
    {
        if (SaveManager.Instance == null) return;
        if (!_processedTransactionIds.Contains(transactionId))
        {
            _processedTransactionIds.Add(transactionId);
            SaveManager.Instance.Data.processedTransactions = new List<string>(_processedTransactionIds);
            SaveManager.Instance.SaveGame();
        }
    }

    private bool IsTransactionProcessed(string transactionId)
    {
        return _processedTransactionIds.Contains(transactionId);
    }
}
