
using UnityEngine;

/// <summary>
/// This power-up attaches a Magnet component to the player, which attracts collectibles.
/// It demonstrates how a PowerUpEffect can be used to manage a component on the player.
/// </summary>
public class MagnetPowerUp : PowerUpEffect
{
    private Magnet magnetComponent;

    public MagnetPowerUp(float duration) : base(duration)
    {
    }

    public override void Activate()
    {
        base.Activate();

        // Get the player instance from the ServiceLocator.
        PlayerController player = ServiceLocator.Get<PlayerController>();
        if (player == null) return;

        // Add the Magnet component to the player if it doesn't already exist.
        // If it does, this power-up will just take control of the existing one.
        magnetComponent = player.gameObject.GetComponent<Magnet>();
        if (magnetComponent == null)
        {
            magnetComponent = player.gameObject.AddComponent<Magnet>();
        }

        magnetComponent.enabled = true;

        // In a more complex game, you might load the upgrade tier from a SaveManager
        // and pass it to the component.
        // magnetComponent.SetUpgradeTier(saveManager.MagnetTier);

        Debug.Log("Magnet PowerUp Activated!");
    }

    public override void Deactivate()
    {
        base.Deactivate();

        // Deactivate the component. Don't destroy it, as another power-up might use it,
        // or it could be a permanent fixture on the player that is just toggled.
        if (magnetComponent != null)
        {
            magnetComponent.enabled = false;
        }

        Debug.Log("Magnet PowerUp Deactivated!");
    }
}
