
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public PowerUpType Type;
    public float duration = 5f;

    public virtual void Activate(GameObject player)
    {
        // Activate the power-up
    }

    public virtual void Deactivate(GameObject player)
    {
        // Deactivate the power-up
    }
}
