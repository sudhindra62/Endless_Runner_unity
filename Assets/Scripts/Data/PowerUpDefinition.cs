using UnityEngine;

/// <summary>
/// ScriptableObject defining a power-up's static properties and logic.
/// Global scope.
/// </summary>
[CreateAssetMenu(fileName = "New PowerUp", menuName = "Endless Runner/Data/Power-Up Definition")]
public class PowerUpDefinition : ScriptableObject
{
    [Header("Core Properties")]
    public PowerUpType type = PowerUpType.None;
    public float duration = 10f;
    public float value = 0f; 

    [Header("UI Properties")]
    public Sprite icon;

    [Header("Visual & Audio Feedback")]
    public GameObject activationEffect;
    public AudioClip activationSound;

    private float remainingDuration;

    public void OnEnable() => remainingDuration = 0;
    public void Activate() => remainingDuration = duration;
    public void Tick(float deltaTime) => remainingDuration -= deltaTime;
    public bool IsActive() => remainingDuration > 0;
    public float GetRemainingDuration() => remainingDuration;
}
