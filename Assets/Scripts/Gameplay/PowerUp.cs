using UnityEngine;

/// <summary>
/// Base class for all active power-up effects.
/// Global scope.
/// </summary>
public abstract class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    public PowerUpType Type => powerUpType;
    public float duration = 10f;
    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}
