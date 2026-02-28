
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
        LoadProcessedTransactions();
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
                CurrencyManager.Instance.AddGems(100);
                success = true;
                break;
            case "com.gamestudio.coin_pack_1":
                CurrencyManager.Instance.AddCoins(5000);
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
        PlayerPrefs.SetInt(RemoveAdsKey, 1);
        PlayerPrefs.Save();
        Debug.Log("Ads have been permanently removed.");
    }

    public bool AreAdsRemoved()
    {
        return PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1;
    }

    private void LoadProcessedTransactions()
    {
        string transactions = PlayerPrefs.GetString(ProcessedTransactionsKey, "");
        if (!string.IsNullOrEmpty(transactions))
        {
            _processedTransactionIds = new List<string>(transactions.Split(','));
        }
    }

    private void AddProcessedTransaction(string transactionId)
    {
        _processedTransactionIds.Add(transactionId);
        PlayerPrefs.SetString(ProcessedTransactionsKey, string.Join(",", _processedTransactionIds));
        PlayerPrefs.Save();
    }

    private bool IsTransactionProcessed(string transactionId)
    {
        return _processedTransactionIds.Contains(transactionId);
    }
}
