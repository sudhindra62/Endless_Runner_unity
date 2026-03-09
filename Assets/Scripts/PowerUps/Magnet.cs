
using UnityEngine;

/// <summary>
/// Concrete implementation for the Coin Magnet power-up.
/// Pulls all nearby coins towards the player for a set duration.
/// Magnetized into existence by the Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(BoxCollider))] // For trigger detection
public class Magnet : PowerUp
{
    [Header("Magnet Specifics")]
    [SerializeField] private float magnetRadius = 15f;
    
    public Magnet()
    {
        powerUpType = PowerUpType.CoinMagnet;
        duration = 12f;
    }

    protected override void Activate(PlayerController player)
    {
        if (player != null)
        {
            player.SetMagnetActive(true, magnetRadius);
        }
    }

    protected override void Deactivate(PlayerController player)
    {
        if (player != null)
        {
            player.SetMagnetActive(false);
        }
    }
}
