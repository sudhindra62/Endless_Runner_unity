
using UnityEngine;

/// <summary>
/// A collectible that awards the player with a bonus number of coins.
/// </summary>
public class BonusCoin : Collectible
{
    [Tooltip("The number of bonus coins to award the player.")]
    [SerializeField] private int coinValue = 10;

    private CurrencyManager currencyManager;

    protected override void Start()
    {
        base.Start();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        poolTag = "BonusCoin";
    }

    protected override void OnCollect()
    {
        if (currencyManager != null)
        {
            currencyManager.AddCoins(coinValue);
        }
    }
}
