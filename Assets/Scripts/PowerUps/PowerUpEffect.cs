/// <summary>
/// Base class for all power-up effects. Manages the duration and activation state of a power-up.
/// </summary>
public abstract class PowerUpEffect
{
    public float Duration { get; private set; }
    public bool IsActive { get; private set; }
    
    private float timer;

    protected PowerUpEffect(float duration)
    {
        Duration = duration;
    }

    public virtual void Activate()
    {
        IsActive = true;
        timer = Duration;
    }

    public virtual void Deactivate()
    {
        IsActive = false;
    }

    public virtual void Update(float deltaTime)
    {
        if (!IsActive) return;

        timer -= deltaTime;
        if (timer <= 0)
        {
            Deactivate();
        }
    }

    public void ResetTimer()
    {
        timer = Duration;
    }

    public float GetRemainingDurationRatio()
    {
        return IsActive ? Mathf.Clamp01(timer / Duration) : 0;
    }
}
