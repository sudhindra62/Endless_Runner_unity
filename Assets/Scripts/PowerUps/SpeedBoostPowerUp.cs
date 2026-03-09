
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

    public override void TriggerActivation(PlayerController player)
    {
        if (player == null) return;
        Debug.Log("Guardian Architect Log: Speed Boost Activated!");
        player.CurrentMoveSpeed = player.BaseMoveSpeed * speedMultiplier;
    }

    public override void TriggerDeactivation(PlayerController player)
    {
        if (player == null) return;
        Debug.Log("Guardian Architect Log: Speed Boost Deactivated.");
        player.CurrentMoveSpeed = player.BaseMoveSpeed;
    }
}
