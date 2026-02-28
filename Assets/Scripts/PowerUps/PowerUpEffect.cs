
using UnityEngine;

/// <summary>
/// A base class for all power-up effects.
/// In a real game, you would create subclasses for specific power-ups like Magnet, Invincibility, etc.
/// </summary>
public abstract class PowerUpEffect
{
    public float Duration { get; protected set; }
    public float TimeRemaining { get; protected set; }
    public bool IsActive { get; protected set; }

    public PowerUpEffect(float duration)
    {
        Duration = duration;
        TimeRemaining = duration;
    }

    /// <summary>
    /// Called once when the power-up is activated.
    /// </summary>
    public virtual void Activate()
    {
        IsActive = true;
        Debug.Log($"{GetType().Name} activated!");
    }

    /// <summary>
    /// Called every frame while the power-up is active.
    /// </summary>
    public virtual void Update(float deltaTime)
    {
        if (!IsActive) return;

        TimeRemaining -= deltaTime;
        if (TimeRemaining <= 0)
        {
            Deactivate();
        }
    }

    /// <summary>
    /// Called once when the power-up expires or is manually deactivated.
    /// </summary>
    public virtual void Deactivate()
    {
        IsActive = false;
        Debug.Log($"{GetType().Name} deactivated!");
    }

    /// <summary>
    /// Resets the timer for this power-up, effectively extending its duration.
    /// </summary>
    public void ResetTimer()
    {
        TimeRemaining = Duration;
    }
}
