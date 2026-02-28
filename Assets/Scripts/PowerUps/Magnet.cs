
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float baseMagnetRadius = 8f;
    [SerializeField] private float magnetForce = 20f;
    [SerializeField] private LayerMask collectibleLayer;

    [Header("VFX & SFX")]
    [SerializeField] private GameObject magnetVisualEffect;
    [SerializeField] private AudioClip magnetSoundLoop;

    private float currentMagnetRadius;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        if (magnetSoundLoop != null) audioSource.clip = magnetSoundLoop;
    }

    private void OnEnable()
    {
        // Example of upgradeable tier - could be loaded from a SaveManager
        SetUpgradeTier(1);
        if (magnetVisualEffect != null) magnetVisualEffect.SetActive(true);
        if (audioSource != null && magnetSoundLoop != null) audioSource.Play();
    }

    private void OnDisable()
    {
        if (magnetVisualEffect != null) magnetVisualEffect.SetActive(false);
        if (audioSource != null) audioSource.Stop();
    }

    private void Update()
    {
        AttractCollectibles();
    }

    private void AttractCollectibles()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentMagnetRadius, collectibleLayer, QueryTriggerInteraction.UseGlobal);

        foreach (Collider hitCollider in colliders)
        {
            if (hitCollider.TryGetComponent<Collectible>(out var collectible))
            {
                // Instead of instantly moving, apply a force or use a lerp in a dedicated script on the coin
                // For simplicity here, we'll use a direct Lerp in the collectible's update.
                // A more robust system would use a pool of movers.
                collectible.Attract(transform);
            }
        }
    }

    public void SetUpgradeTier(int tier)
    {
        // Example: Radius scales with tier
        currentMagnetRadius = baseMagnetRadius * (1 + (tier - 1) * 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentMagnetRadius);
    }
}
