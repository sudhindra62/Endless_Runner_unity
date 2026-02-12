
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages In-App Purchases (IAP) with a focus on reliability and fail-safe handling.
/// </summary>
public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }

    private enum PurchaseStatus { Idle, Pending, Successful, Failed }
    private class PurchaseTransaction
    {
        public string ProductId;
        public PurchaseStatus Status;
        public Action OnSuccess;
        public Action OnFailure;
        public float Timeout;
    }

    private readonly Dictionary<string, PurchaseTransaction> activeTransactions = new Dictionary<string, PurchaseTransaction>();
    private const float PurchaseTimeout = 15f; // 15 seconds

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Monitor for timed-out transactions
        MonitorTimeouts();
    }

    public void InitiatePurchase(string productId, Action onSuccess, Action onFailure)
    {
        if (activeTransactions.ContainsKey(productId) && activeTransactions[productId].Status == PurchaseStatus.Pending)
        {
            Debug.LogWarning($"[IAPManager] Purchase for {productId} is already pending.");
            onFailure?.Invoke();
            return;
        }

        var transaction = new PurchaseTransaction
        {
            ProductId = productId,
            Status = PurchaseStatus.Pending,
            OnSuccess = onSuccess,
            OnFailure = onFailure,
            Timeout = Time.time + PurchaseTimeout
        };

        activeTransactions[transaction.ProductId] = transaction;
        Debug.Log($"[IAPManager] Initiating purchase for: {productId}");

        // --- This is where you would call the actual IAP SDK (e.g., Unity IAP) ---
        // UnityIAP.InitiatePurchase(productId);

        // For simulation, we'll directly call the validation
        SimulatePurchaseFlow(transaction);
    }

    private void SimulatePurchaseFlow(PurchaseTransaction transaction)
    {
        // 1. Simulate the IAP system returning a receipt
        string simulatedReceipt = "receipt_for_" + transaction.ProductId + "_" + Guid.NewGuid().ToString();

        // 2. Start validation
        PurchaseValidator.Instance.ValidatePurchase(transaction.ProductId, simulatedReceipt, (isValid) =>
        {
            if (isValid)
            {
                ProcessSuccessfulPurchase(transaction.ProductId);
            }
            else
            {
                ProcessFailedPurchase(transaction.ProductId, "Validation Failed");
            }
        });
    }

    public void ProcessSuccessfulPurchase(string productId)
    {
        if (activeTransactions.TryGetValue(productId, out var transaction) && transaction.Status == PurchaseStatus.Pending)
        {
            Debug.Log($"[IAPManager] Purchase successful for {productId}. Granting item.");
            transaction.Status = PurchaseStatus.Successful;

            // --- Grant the item ---
            GrantItem(productId);

            transaction.OnSuccess?.Invoke();
            activeTransactions.Remove(productId);
        }
    }

    public void ProcessFailedPurchase(string productId, string reason)
    {
        if (activeTransactions.TryGetValue(productId, out var transaction) && transaction.Status == PurchaseStatus.Pending)
        {
            Debug.LogError($"[IAPManager] Purchase failed for {productId}. Reason: {reason}");
            transaction.Status = PurchaseStatus.Failed;
            transaction.OnFailure?.Invoke();
            activeTransactions.Remove(productId);
        }
    }

    private void GrantItem(string productId)
    {
        // This is where you connect the purchase to the game's reward system.
        // For example:
        if (productId.StartsWith("gems_"))
        {
            int amount = int.Parse(productId.Split('_')[1]);
            CurrencyManager.Instance.AddGems(amount);
        }
        else if (productId == "remove_ads")
        {
            // AdsManager.Instance.RemoveAds();
        }
        // etc.
    }

    private void MonitorTimeouts()
    {
        List<string> timedOutTransactions = new List<string>();
        foreach (var trans in activeTransactions.Values)
        {
            if (trans.Status == PurchaseStatus.Pending && Time.time > trans.Timeout)
            {
                timedOutTransactions.Add(trans.ProductId);
            }
        }

        foreach (string productId in timedOutTransactions)
        {
            ProcessFailedPurchase(productId, "Purchase timed out.");
        }
    }

    /// <summary>
    /// Allows retrying a failed purchase.
    /// </summary>
    public void RetryPurchase(string productId)
    {
        if (activeTransactions.TryGetValue(productId, out var transaction))
        {
            Debug.Log($"[IAPManager] Retrying purchase for {productId}");
            InitiatePurchase(productId, transaction.OnSuccess, transaction.OnFailure);
        }
    }
}
