using UnityEngine;

/// <summary>
/// ScriptableObject that defines the configuration for a power-up.
/// This allows for easy tweaking of power-up properties in the Unity Inspector.
/// </summary>
[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUps/PowerUp Config")]
public class PowerUp : ScriptableObject
{
    [Header("Configuration")]
    [SerializeField] private PowerUpType type;
    [SerializeField] private float duration = 10f;

    public PowerUpType Type => type;
    public float Duration => duration;
}
