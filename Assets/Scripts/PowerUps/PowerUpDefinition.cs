
using UnityEngine;

/// <summary>
/// Defines a power-up using a ScriptableObject for easy configuration.
/// This allows for creating different power-ups as assets in the project.
/// </summary>
[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Gameplay/PowerUp Definition")]
public class PowerUpDefinition : ScriptableObject
{
    [Tooltip("The unique type of this power-up.")]
    public PowerUpType type;

    [Tooltip("The duration this power-up remains active, in seconds.")]
    public float duration;

    [Tooltip("The value or magnitude of the power-up's effect (e.g., score multiplier amount).")]
    public float value;

    [Tooltip("Visual effect to spawn when the power-up is activated.")]
    public GameObject activationEffect;
}
