
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

    public void SetBloom(bool enabled)
    {
        if (bloom != null) bloom.active = enabled;
    }

    public void SetMotionBlur(bool enabled)
    {
        if (motionBlur != null) motionBlur.active = enabled;
    }

    public void SetColorGrading(bool enabled)
    {
        if (colorGrading != null) colorGrading.active = enabled;
    }

    public void SetDepthOfField(bool enabled)
    {
        if (depthOfField != null) depthOfField.active = enabled;
    }
}
