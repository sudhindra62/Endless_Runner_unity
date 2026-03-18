
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager Instance { get; private set; }

    [Header("Post-Processing Settings")]
    [SerializeField] private Volume postProcessingVolume;

    private Bloom bloom;
    private MotionBlur motionBlur;
    private ColorAdjustments colorGrading;
    private DepthOfField depthOfField;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out bloom);
            postProcessingVolume.profile.TryGet(out motionBlur);
            postProcessingVolume.profile.TryGet(out colorGrading);
            postProcessingVolume.profile.TryGet(out depthOfField);
        }
    }

    public void SetBloom(bool enabled, float intensity = 1.0f)
    {
        if (bloom != null) 
        {
            bloom.active = enabled;
            bloom.intensity.value = intensity;
        }
    }

    public void SetMotionBlur(bool enabled)
    {
        if (motionBlur != null) motionBlur.active = enabled;
    }

    public void SetColorGrading(bool enabled, float postExposure = 0, float contrast = 0, float saturation = 0)
    {
        if (colorGrading != null) 
        {
            colorGrading.active = enabled;
            colorGrading.postExposure.value = postExposure;
            colorGrading.contrast.value = contrast;
            colorGrading.saturation.value = saturation;
        }
    }

    public void SetDepthOfField(bool enabled, float focusDistance = 10f, float aperture = 5.6f)
    {
        if (depthOfField != null) 
        {
            depthOfField.active = enabled;
            depthOfField.focusDistance.value = focusDistance;
            depthOfField.aperture.value = aperture;
        }
    }
}
