using UnityEngine;
using System.Collections;

/// <summary>
/// This component manages the lifecycle of the fusion core visual effect. It plays a particle effect,
/// scales up and down, and then destroys itself. This is used to provide visual feedback for a power-up fusion.
/// </summary>
public class FusionCore : MonoBehaviour
{
    [Header("VFX & SFX")]
    [Tooltip("The particle system to play on start.")]
    [SerializeField] private ParticleSystem coreParticles;

    [Tooltip("The audio clip to play on start.")]
    [SerializeField] private AudioClip fusionSound;

    [Header("Animation")]
    [Tooltip("The duration of the scale-up and scale-down animation.")]
    [SerializeField] private float animationDuration = 1.5f;
    [Tooltip("The maximum scale the core will reach.")]
    [SerializeField] private float maxScale = 2.0f;

    private AudioSource audioSource;

    private void Start()
    {
        // Ensure there is an AudioSource component to play the sound
        audioSource = gameObject.AddComponent<AudioSource>();
        if (fusionSound != null)
        {
            audioSource.PlayOneShot(fusionSound);
        }

        // Play the particle effect
        if (coreParticles != null)
        {
            coreParticles.Play();
        }

        // Start the scale animation
        StartCoroutine(AnimateScale());
    }

    /// <summary>
    /// Coroutine to animate the scale of the GameObject, making it pulse.
    /// </summary>
    private IEnumerator AnimateScale()
    {        
        float halfDuration = animationDuration / 2;
        
        // Scale up
        float timer = 0f;
        while (timer < halfDuration)
        {
            float scale = Mathf.Lerp(0, maxScale, timer / halfDuration);
            transform.localScale = Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one * maxScale;

        // Scale down
        timer = 0f;
        while (timer < halfDuration)
        {
            float scale = Mathf.Lerp(maxScale, 0, timer / halfDuration);
            transform.localScale = Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure it ends at zero scale and then destroy
        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
