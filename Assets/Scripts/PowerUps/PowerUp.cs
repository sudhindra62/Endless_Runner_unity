using UnityEngine;
using System;

public abstract class PowerUp : MonoBehaviour
{
    public static event Action<PowerUp> OnPowerUpCompleted;

    [Header("Configuration")]
    [SerializeField] private float duration = 5f;

    protected float Timer { get; private set; }

    public virtual void ApplyPowerUp() { }

    public virtual void RemovePowerUp() { }

    protected void CompletePowerUp()
    {
        OnPowerUpCompleted?.Invoke(this);
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= duration)
        {
            RemovePowerUp();
            CompletePowerUp();
        }
    }
}
