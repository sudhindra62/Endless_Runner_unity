
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

/// <summary>
/// Manages premium visual effects to enhance the player experience.
/// This includes particle effects, full-screen effects, and other visual enhancements.
/// </summary>
public class PremiumVisualsManager : Singleton<PremiumVisualsManager>
{
    [Header("Impact Burst Settings")]
    public GameObject impactBurstPrefab;
    public float impactBurstLifetime = 2f;

    [Header("Vibrance Protocol Settings")]
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    private List<ParticleSystem> activeEffects = new List<ParticleSystem>();

    void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            // Successfully fetched ColorAdjustments
        }
    }

    /// <summary>
    /// Plays an impact burst effect at the specified position.
    /// </summary>
    /// <param name="position">The world position to play the effect.</param>
    public void PlayImpactBurst(Vector3 position)
    {
        if (impactBurstPrefab != null)
        {
            GameObject burst = Instantiate(impactBurstPrefab, position, Quaternion.identity);
            ParticleSystem ps = burst.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                activeEffects.Add(ps);
                var main = ps.main;
                main.stopAction = ParticleSystemStopAction.Destroy;
                ps.Play();
            }
            else
            {
                Destroy(burst, impactBurstLifetime);
            }
        }
    }

    /// <summary>
    /// Toggles the vibrance protocol on or off.
    /// </summary>
    /// <param name="isEnabled">Whether to enable or disable the vibrance protocol.</param>
    public void SetVibranceProtocol(bool isEnabled)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.active = isEnabled;
        }
    }

    /// <summary>
    /// Sets the intensity of the vibrance protocol.
    /// </summary>
    /// <param name="intensity">The intensity of the vibrance effect, from 0 to 1.</param>
    public void SetVibranceIntensity(float intensity)
    {
        if (colorAdjustments != null)
        {
            float postExposureValue = Mathf.Lerp(0, 1.5f, intensity);
            float contrastValue = Mathf.Lerp(0, 50, intensity);
            colorAdjustments.postExposure.value = postExposureValue;
            colorAdjustments.contrast.value = contrastValue;
        }
    }

    /// <summary>
    /// Stops all currently active premium visual effects.
    /// </summary>
    public void ClearAllEffects()
    {
        foreach (var effect in activeEffects)
        {
            if (effect != null)
            {
                effect.Stop();
            }
        }
        activeEffects.Clear();
    }
}
