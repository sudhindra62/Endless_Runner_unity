using UnityEngine;

/// <summary>
/// The Coin Storm fusion power-up. This is a combination of the Magnet and Coin Doubler.
/// It activates a powerful magnet and triples the value of all collected coins.
/// </summary>
public class CoinStormPowerUp : PowerUpEffect
{
    private Magnet magnet;
    private CurrencyManager currencyManager;
    private NewCollectibleSpawner collectibleSpawner;

    private readonly float magnetRange; // The enhanced range for the magnet
    private readonly float spawnChanceMultiplier;

    public CoinStormPowerUp(float duration, float range, float spawnMultiplier) : base(duration)
    {
        this.magnetRange = range;
        this.spawnChanceMultiplier = spawnMultiplier;
        magnet = ServiceLocator.Get<PlayerController>()?.GetComponentInChildren<Magnet>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        collectibleSpawner = ServiceLocator.Get<NewCollectibleSpawner>();
    }

    public override void Activate()
    {
        base.Activate();
        if (magnet != null)
        {
            magnet.SetRange(magnetRange);
            magnet.StartMagnet();
        }

        if (currencyManager != null)
        {
            currencyManager.SetCoinMultiplier(3); // Triple coin value
        }

        if (collectibleSpawner != null)
        {
            collectibleSpawner.SetSpawnChanceMultiplier(spawnChanceMultiplier);
        }

        Debug.Log("Coin Storm Activated! All coins are drawn in and tripled in value.");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (magnet != null)
        {
            magnet.StopMagnet();
        }

        if (currencyManager != null)
        {
            currencyManager.SetCoinMultiplier(1); // Reset to default
        }

        if (collectibleSpawner != null)
        {
            collectibleSpawner.SetSpawnChanceMultiplier(1f); // Reset to default
        }

        Debug.Log("Coin Storm Deactivated.");
    }
}
