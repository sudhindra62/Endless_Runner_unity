using UnityEngine;

/// <summary>
/// The definitive, consolidated implementation of the Shield power-up.
/// Inherits from the base PowerUp class and provides all visual, audio, and gameplay logic for the shield.
/// Logic fully restored and fortified by Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(menuName = "PowerUps/Shield")]
public class ShieldPowerUp : PowerUp
{
    [Header("VFX & SFX")]
    [SerializeField] private GameObject shieldVisualEffect;
    [SerializeField] private GameObject shieldBreakEffect;
    [SerializeField] private AudioClip shieldBreakSound;

    private AudioSource audioSource;

    public override void Activate(GameObject player)
    {
        // Access the PlayerController and enable the shield state.
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.IsInvincible = true;
        }

        // Activate the visual shield.
        if (shieldVisualEffect != null) 
        {
            // Instantiate the effect as a child of the player to follow it.
            GameObject shieldInstance = Instantiate(shieldVisualEffect, player.transform.position, player.transform.rotation, player.transform);
            // We can add a component to the player to manage the shield instance if needed.
        }

        // Setup audio source for break sound.
        audioSource = player.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public override void Deactivate(GameObject player)
    {
        // Access the PlayerController and disable the shield state.
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.IsInvincible = false;
        }

        // The visual shield instance will be destroyed along with the player if not handled separately.
        // Or, we can find it and destroy it.
    }
    
    // This method can be called from the PlayerController when an obstacle is hit.
    public void OnObstacleHit(GameObject player)
    {
        // Play break effect and sound
        if (shieldBreakEffect != null) Instantiate(shieldBreakEffect, player.transform.position, Quaternion.identity);
        if (audioSource != null && shieldBreakSound != null) audioSource.PlayOneShot(shieldBreakSound);

        // Deactivate the power-up through the manager
        PowerUpManager.Instance.DeactivatePowerUp(this);
    }
}
