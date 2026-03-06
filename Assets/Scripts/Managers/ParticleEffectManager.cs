using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    public static ParticleEffectManager Instance { get; private set; }

    private float globalParticleMultiplier = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetGlobalParticleMultiplier(float multiplier)
    {
        globalParticleMultiplier = Mathf.Clamp01(multiplier);
        // This is a simplified approach. A more robust solution would be to
        // find all ParticleSystem components and adjust their emission rates,
        // or have particle effects query this manager for the multiplier when they are played.
        Debug.Log($"Global particle effect multiplier set to {globalParticleMultiplier}");
    }

    public float GetGlobalParticleMultiplier()
    {
        return globalParticleMultiplier;
    }
}
