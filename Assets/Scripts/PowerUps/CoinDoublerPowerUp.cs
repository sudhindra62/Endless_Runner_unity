using UnityEngine;

/// <summary>
/// This power-up doubles the value of all coins collected.
/// It activates by setting the coin multiplier in the CurrencyManager to 2.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/CoinDoubler")]
public class CoinDoublerPowerUp : PowerUp
{
    public override void Activate(GameObject player)
    {
        CurrencyManager currencyManager = ServiceLocator.Get<CurrencyManager>();
        if (currencyManager != null)
        {
            currencyManager.ActivateCoinDoubler(true);
        }
    }

    public override void Deactivate(GameObject player)
    {
        CurrencyManager currencyManager = ServiceLocator.Get<CurrencyManager>();
        if (currencyManager != null)
        {
            currencyManager.ActivateCoinDoubler(false);
        }
    }
}
