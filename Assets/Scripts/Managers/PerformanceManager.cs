
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager Instance { get; private set; }

    public enum QualityLevel { Low, Medium, High }

    [Header("Performance Settings")]
    [Tooltip("Target frame rate for the application, crucial for mobile optimization.")]
    [SerializeField] private int targetFrameRate = 60;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = targetFrameRate;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetQualityLevel(QualityLevel level)
    {
        switch (level)
        {
            case QualityLevel.Low:
                QualitySettings.SetQualityLevel(0, true);
                SetLODBias(0.7f);
                SetShaderComplexity(QualityLevel.Low);
                PostProcessingManager.Instance?.SetBloom(false);
                PostProcessingManager.Instance?.SetMotionBlur(false);
                break;
            case QualityLevel.Medium:
                QualitySettings.SetQualityLevel(1, true);
                SetLODBias(1.0f);
                SetShaderComplexity(QualityLevel.Medium);
                PostProcessingManager.Instance?.SetBloom(true, 0.5f);
                PostProcessingManager.Instance?.SetMotionBlur(false);
                break;
            case QualityLevel.High:
                QualitySettings.SetQualityLevel(2, true);
                SetLODBias(2.0f);
                SetShaderComplexity(QualityLevel.High);
                PostProcessingManager.Instance?.SetBloom(true, 1.0f);
                PostProcessingManager.Instance?.SetMotionBlur(true);
                break;
        }
    }

    public void SetLODBias(float bias)
    {
        QualitySettings.lodBias = bias;
    }

    public void SetShaderComplexity(QualityLevel level)
    {
        // Placeholder for logic to switch shaders based on quality level
    }
    
    public void SetTargetFrameRate(int rate)
    {
        targetFrameRate = rate;
        Application.targetFrameRate = targetFrameRate;
    }
}
