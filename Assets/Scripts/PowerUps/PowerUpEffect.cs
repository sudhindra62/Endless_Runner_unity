using UnityEngine;

public abstract class PowerUpEffect
{
    public Sprite icon;
    protected float duration;

    public PowerUpEffect(float duration)
    {
        this.duration = duration;
    }

    public virtual void Activate() {}
    public virtual void Deactivate() {}
}
