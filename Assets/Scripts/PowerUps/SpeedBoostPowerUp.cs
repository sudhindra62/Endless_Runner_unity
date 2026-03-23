
using UnityEngine;

/// <summary>
/// Implements the Speed Boost power-up, temporarily increasing the player's run speed.
/// This script communicates with the PlayerController to modify its speed properties.
/// Created and fortified by Supreme Guardian Architect v12.
/// </summary>
public class SpeedBoostPowerUp : PowerUp
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 1.5f;

    void Awake()
    {
        powerUpType = PowerUpType.SpeedBoost;
    }

    public override void ApplyEffect()
    {
        PlayerController player = PlayerController.Instance;
        if (player == null) return;
        Debug.Log("Guardian Architect Log: Speed Boost Activated!");
        player.SetSpeed(player.BaseMoveSpeed * speedMultiplier);
    }

    public override void RemoveEffect()
    {
        PlayerController player = PlayerController.Instance;
        if (player == null) return;
        Debug.Log("Guardian Architect Log: Speed Boost Deactivated.");
        player.SetSpeed(player.BaseMoveSpeed);
    }
}
