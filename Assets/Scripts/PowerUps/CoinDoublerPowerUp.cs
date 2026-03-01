
/// <summary>
/// This power-up doubles the value of all coins collected.
/// It activates by setting the coin multiplier in the CurrencyManager to 2.
/// </summary>
public class CoinDoublerPowerUp : PowerUpEffect
{
    private CurrencyManager currencyManager;

    public CoinDoublerPowerUp(float duration) : base(duration)
    {
        currencyManager = ServiceLocator.Get<CurrencyManager>();
    }

    public override void Activate()
    {
        base.Activate();
        if (currencyManager != null)
        {
            currencyManager.SetCoinMultiplier(2);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (currencyManager != null)
        {
            currencyManager.SetCoinMultiplier(1); // Reset to default
        }
    }
}
