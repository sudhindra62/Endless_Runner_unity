
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager Instance { get; private set; }

    public enum QualityLevel { Low, Medium, High }

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
    }

    public void SetQualityLevel(QualityLevel level)
    {
        switch (level)
        {
            case QualityLevel.Low:
                QualitySettings.SetQualityLevel(0, true);
                QualitySettings.lodBias = 0.5f;
                break;
            case QualityLevel.Medium:
                QualitySettings.SetQualityLevel(1, true);
                QualitySettings.lodBias = 1.0f;
                break;
            case QualityLevel.High:
                QualitySettings.SetQualityLevel(2, true);
                QualitySettings.lodBias = 2.0f;
                break;
        }
    }
}
