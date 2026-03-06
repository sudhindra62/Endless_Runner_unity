using UnityEngine;

[CreateAssetMenu(fileName = "DynamicQualityProfile", menuName = "Performance/Dynamic Quality Profile")]
public class DynamicQualityProfile : ScriptableObject
{
    [Header("Post-Processing")]
    public bool bloomEnabled = true;

    [Header("Particles")]
    [Range(0.1f, 1.0f)]
    public float particleEffectMultiplier = 1.0f;

    [Header("Rendering")]
    public ShadowQuality shadowQuality = ShadowQuality.All;
    [Range(0.2f, 1.0f)]
    public float lodBias = 1.0f;

    [Header("Environment")]
    [Range(0.0f, 1.0f)]
    public float decorationDensity = 1.0f;
}
