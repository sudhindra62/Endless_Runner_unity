
/// <summary>
/// The concrete implementation of the Shield power-up effect.
/// It inherits from PowerUpEffect and handles the logic for activating and deactivating the player's shield.
/// </summary>
public class ShieldPowerUp : PowerUpEffect
{
    private PlayerController player;

    public ShieldPowerUp(float duration) : base(duration)
    {
        player = ServiceLocator.Get<PlayerController>();
    }

    public override void Activate()
    {
        base.Activate();
        if (player != null)
        {
            // Assuming PlayerController has a method to set the shield status.
            // This will need to be implemented in PlayerController.
            player.SetShield(true);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (player != null)
        {
            player.SetShield(false);
        }
    }
}
