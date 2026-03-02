
using UnityEngine;

/// <summary>
/// A self-managing component that listens for Magnet power-up events 
/// and handles the attraction of nearby collectibles.
/// </summary>
public class Magnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float baseMagnetRadius = 8f;
    [SerializeField] private LayerMask collectibleLayer;

    [Header("VFX & SFX")]
    [SerializeField] private GameObject magnetVisualEffect;
    [SerializeField] private AudioClip magnetSoundLoop;

    private float currentMagnetRadius;
    private AudioSource audioSource;
    private bool isMagnetActive = false;

    private void Awake()
    {
        // Set up the audio source for the magnet loop
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        if (magnetSoundLoop != null) audioSource.clip = magnetSoundLoop;
    }

    private void Start()
    {
        // Subscribe to power-up events to self-manage state
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivation;
            PowerUpManager.Instance.OnPowerUpExpired += HandlePowerUpExpiration;
        }

        // Ensure magnet is off by default
        if (magnetVisualEffect != null) magnetVisualEffect.SetActive(false);
        isMagnetActive = false;

        // Set a default radius tier
        SetUpgradeTier(1);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivation;
            PowerUpManager.Instance.OnPowerUpExpired -= HandlePowerUpExpiration;
        }
    }

    private void Update()
    {
        // Only run attraction logic if the power-up is active
        if (isMagnetActive)
        {
            AttractCollectibles();
        }
    }

    private void HandlePowerUpActivation(PowerUp powerUp, float duration)
    {
        if (powerUp.Type == PowerUpType.Magnet)
        {
            isMagnetActive = true;
            if (magnetVisualEffect != null) magnetVisualEffect.SetActive(true);
            if (audioSource != null && magnetSoundLoop != null) audioSource.Play();
        }
    }

    private void HandlePowerUpExpiration(PowerUp powerUp)
    {
        if (powerUp.Type == PowerUpType.Magnet)
        {
            isMagnetActive = false;
            if (magnetVisualEffect != null) magnetVisualEffect.SetActive(false);
            if (audioSource != null) audioSource.Stop();
        }
    }

    private void AttractCollectibles()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentMagnetRadius, collectibleLayer, QueryTriggerInteraction.UseGlobal);

        foreach (Collider hitCollider in colliders)
        {
            if (hitCollider.TryGetComponent<Collectible>(out var collectible))
            {
                collectible.Attract(transform);
            }
        }
    }

    public void SetUpgradeTier(int tier)
    {
        // Radius can be scaled based on a loaded upgrade level
        currentMagnetRadius = baseMagnetRadius * (1 + (tier - 1) * 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentMagnetRadius);
    }
}
