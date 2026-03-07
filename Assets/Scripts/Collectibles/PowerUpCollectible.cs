
using UnityEngine;

public class PowerUpCollectible : Collectible
{
    [Header("Power-Up")]
    [SerializeField] private PowerUp powerUp;

    protected override void OnCollect()
    {
        base.OnCollect();

        // Activate the power-up
        powerUp.Activate(GameObject.FindGameObjectWithTag("Player"));

        // Deactivate the power-up after a certain duration
        Invoke(nameof(DeactivatePowerUp), powerUp.duration);
    }

    private void DeactivatePowerUp()
    {
        powerUp.Deactivate(GameObject.FindGameObjectWithTag("Player"));
    }
}
