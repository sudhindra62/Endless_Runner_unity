
using UnityEngine;

/// <summary>
/// A collectible that grants the player a temporary shield, protecting them from a single obstacle hit.
/// </summary>
public class Shield : Collectible
{
    private PowerUpManager powerupManager;

    protected override void Start()
    {
        base.Start();
        powerupManager = ServiceLocator.Get<PowerUpManager>();
        poolTag = "Shield";
    }

    protected override void OnCollect()
    {
        if(powerupManager != null)
        {
            powerupManager.ActivateShield();
        }
    }
}
