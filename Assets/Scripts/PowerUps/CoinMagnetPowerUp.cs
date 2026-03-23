
using UnityEngine;

/// <summary>
/// Implements the Coin Magnet power-up, which attracts nearby coins to the player.
/// This script communicates with the PlayerController to activate its magnet radius.
/// Created and fortified by Supreme Guardian Architect v12.
/// </summary>
public class CoinMagnetPowerUp : PowerUp
{
    [Header("Coin Magnet Settings")]
    [SerializeField] private float magnetRadius = 5f;

    void Awake()
    {
        powerUpType = PowerUpType.Magnet;
    }

    public override void ApplyEffect()
    {
        PlayerController player = PlayerController.Instance;
        if (player == null) return;
        Debug.Log("Guardian Architect Log: Coin Magnet Activated!");
        player.SetMagnetActive(true, magnetRadius);
    }

    public override void RemoveEffect()
    {
        PlayerController player = PlayerController.Instance;
        if (player == null) return;
        Debug.Log("Guardian Architect Log: Coin Magnet Deactivated.");
        player.SetMagnetActive(false, 0f);
    }
}
