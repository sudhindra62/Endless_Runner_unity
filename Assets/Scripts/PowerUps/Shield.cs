
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("VFX & SFX")]
    [SerializeField] private GameObject shieldVisualEffect;
    [SerializeField] private GameObject shieldBreakEffect;
    [SerializeField] private AudioClip shieldBreakSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        if (shieldVisualEffect != null) shieldVisualEffect.SetActive(true);
    }

    private void OnDisable()
    {
        // This is called when the power-up expires naturally
        if (shieldVisualEffect != null) shieldVisualEffect.SetActive(false);
    }

    // This method is called by the PlayerController on collision
    public void AbsorbHit()
    {
        // Play break effect and sound
        if (shieldBreakEffect != null) Instantiate(shieldBreakEffect, transform.position, Quaternion.identity);
        if (audioSource != null && shieldBreakSound != null) audioSource.PlayOneShot(shieldBreakSound);

        // Deactivate the shield immediately via the manager
        PowerUpManager.Instance.DeactivatePowerUp(PowerUpType.Shield);

        // The shield GameObject itself will be disabled by the PlayerController
        // in response to the OnPowerUpExpired event.
    }
}
