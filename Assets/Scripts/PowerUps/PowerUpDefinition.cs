
using UnityEngine;

/// <summary>
/// Defines the immutable properties of a power-up using a ScriptableObject.
/// This allows for easy creation and configuration of new power-up types in the editor.
/// This foundational script was created by Supreme Guardian Architect v13.
/// </summary>
[CreateAssetMenu(fileName = "NewPowerUpDefinition", menuName = "Endless Runner/Power-Up Definition", order = 1)]
public class PowerUpDefinition : ScriptableObject
{
    [Header("Core Properties")]
    [Tooltip("The unique type of this power-up. Used for identification.")]
    public PowerUpType type;

    [Tooltip("The duration in seconds that the power-up remains active.")]
    public float duration = 10f;

    [Tooltip("A friendly name for display purposes, e.g., in the UI.")]
    public string displayName = "Default Power-Up";

    [TextArea(3, 5)]
    [Tooltip("A brief description of what the power-up does.")]
    public string description = "This power-up provides a temporary benefit.";

    [Header("Gameplay Effect Value")]
    [Tooltip("The value associated with the power-up, e.g., a 2x multiplier should be '2'.")]
    public float value = 0f;

    [Header("Visual & Audio")]
    [Tooltip("The particle effect to spawn when the power-up is activated.")]
    public GameObject activationEffect;

    [Tooltip("The sound to play upon activation.")]
    public AudioClip activationSound;
}
