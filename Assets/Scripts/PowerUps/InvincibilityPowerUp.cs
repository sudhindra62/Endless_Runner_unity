using UnityEngine;

public class InvincibilityPowerUp : PowerUp
{
    public override void ApplyPowerUp()
    {
        base.ApplyPowerUp();
        // The player controller will check for this power-up and ignore damage
    }

    public override void RemovePowerUp()
    {
        base.RemovePowerUp();
    }
}
