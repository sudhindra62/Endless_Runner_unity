using UnityEngine;

/// <summary>
/// Unified API for Particle Effects and VFX.
/// </summary>
public class ParticleEffectManager : Singleton<ParticleEffectManager>
{
    private float globalParticleMultiplier = 1f;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register(this);
    }

    public void PlayEffect(string effectType, Vector3 position, Quaternion rotation)
    {
        Debug.Log($"Guardian Architect: Playing effect {effectType} at {position}");
        // Integration with ObjectPooler and VFXManager
        if (ObjectPooler.Instance != null)
        {
            GameObject effect = ObjectPooler.Instance.GetPooledObject(effectType);
            if (effect != null)
            {
                effect.transform.position = position;
                effect.transform.rotation = rotation;
                effect.SetActive(true);
            }
        }
    }

    public void SetGlobalParticleMultiplier(float multiplier)
    {
        globalParticleMultiplier = Mathf.Max(0f, multiplier);
    }

    public float GetGlobalParticleMultiplier() => globalParticleMultiplier;
}
