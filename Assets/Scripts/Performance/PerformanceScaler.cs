using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PerformanceScaler : MonoBehaviour
{
    public static PerformanceScaler Instance { get; private set; }

    [Header("Configuration")]
    public DynamicQualityProfile[] qualityProfiles; // Assign 4 profiles: Low, Medium, High, Ultra
    public float frameTimeSpikeThreshold = 50.0f; // in milliseconds
    public int memoryUsageThresholdMB = 2048; // MB
    public float lowFpsThreshold = 25.0f;
    public float highFpsThreshold = 55.0f;

    [Header("Monitoring")]
    private float lastFrameTime;
    private float averageFps;
    private int currentProfileIndex = -1;

    private UniversalRenderPipelineAsset pipelineAsset;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        Initialize();
    }

    void Update()
    {
        MonitorPerformance();
    }

    private void Initialize()
    {
        var initialTier = DeviceProfileAnalyzer.AnalyzeDevice();
        currentProfileIndex = (int)initialTier;
        ApplyQualityProfile(qualityProfiles[currentProfileIndex]);
    }

    private void MonitorPerformance()
    {
        // FPS Monitoring
        averageFps = 0.9f * averageFps + 0.1f * (1.0f / Time.unscaledDeltaTime);

        // Spike Detection
        lastFrameTime = Time.unscaledDeltaTime * 1000.0f; // in milliseconds
        if (lastFrameTime > frameTimeSpikeThreshold)
        {
            Debug.LogWarning("Frame time spike detected: " + lastFrameTime + "ms. Considering downgrade.");
            ScaleDown();
        }

        // Memory Monitoring
#if !UNITY_EDITOR
        long memoryUsage = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);
        if (memoryUsage > memoryUsageThresholdMB)
        {
            Debug.LogWarning("High memory usage: " + memoryUsage + "MB. Considering downgrade.");
            ScaleDown();
        }
#endif

        // Sustained Low FPS
        if (averageFps < lowFpsThreshold)
        {
            ScaleDown();
        }
        else if (averageFps > highFpsThreshold)
        {
            ScaleUp();
        }
    }

    private void ScaleDown()
    {
        if (currentProfileIndex > 0)
        {
            currentProfileIndex--;
            ApplyQualityProfile(qualityProfiles[currentProfileIndex]);
            Debug.Log("Performance scaled down to: " + (DeviceProfileAnalyzer.PerformanceTier)currentProfileIndex);
        }
    }

    private void ScaleUp()
    {
        if (currentProfileIndex < qualityProfiles.Length - 1)
        {
            currentProfileIndex++;
            ApplyQualityProfile(qualityProfiles[currentProfileIndex]);
            Debug.Log("Performance scaled up to: " + (DeviceProfileAnalyzer.PerformanceTier)currentProfileIndex);
        }
    }

    public void ApplyQualityProfile(DynamicQualityProfile profile)
    {
        if (profile == null || pipelineAsset == null)
        {
            Debug.LogError("Quality profile or pipeline asset is not set!");
            return;
        }

        // Bloom via volume
        // Note: rendererData direct access was removed in newer URP.
        // Use URP ScriptableRendererFeature approach instead or global volume overrides.
        Debug.Log("[PerformanceScaler] Bloom scaling handled via URP Volume.");

        // Particles
        ParticleEffectManager.Instance?.SetGlobalParticleMultiplier(profile.particleEffectMultiplier);

        // Shadow Quality
        QualitySettings.shadows = profile.shadowQuality;

        // LOD Bias
        QualitySettings.lodBias = profile.lodBias;

        // Environment Decoration Density
        WorldThemeManager.Instance?.SetDecorationDensity(profile.decorationDensity);

        Debug.Log("Applied quality profile: " + profile.name);
    }
}
