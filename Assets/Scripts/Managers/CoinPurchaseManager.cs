
using UnityEngine;

/// <summary>
/// Manages the purchasing of coins.
/// </summary>
public class CoinPurchaseManager : MonoBehaviour
{
    public static CoinPurchaseManager Instance { get; private set; }

    private IAPManager _iapManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<CoinPurchaseManager>(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            ServiceLocator.Unregister<CoinPurchaseManager>();
            Instance = null;
        }
    }

    private void Start()
    {
        _iapManager = ServiceLocator.Get<IAPManager>();
    }

    public void PurchaseCoins(string coinProductId)
    {
        _iapManager.InitiatePurchase(coinProductId);
    }
}
