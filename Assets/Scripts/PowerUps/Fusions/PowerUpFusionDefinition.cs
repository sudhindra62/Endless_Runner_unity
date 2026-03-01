using UnityEngine;

/// <summary>
/// Defines a recipe for a power-up fusion. It specifies the two power-ups that are required
/// to create a new, more powerful fused effect.
/// </summary>
[CreateAssetMenu(fileName = "PowerUpFusion", menuName = "PowerUps/Power-Up Fusion Definition")]
public class PowerUpFusionDefinition : ScriptableObject
{
    [Header("Fusion Inputs")]
    [Tooltip("The first power-up required for this fusion.")]
    public PowerUpType powerUp1;

    [Tooltip("The second power-up required for this fusion.")]
    public PowerUpType powerUp2;

    [Header("Fusion Output")]
    [Tooltip("The resulting fused power-up effect.")]
    public PowerUpType fusedPowerUp;

    [Tooltip("The duration of the fused power-up effect.")]
    public float fusedDuration;
}
