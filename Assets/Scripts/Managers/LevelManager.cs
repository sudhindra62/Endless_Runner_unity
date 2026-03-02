
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // --- Difficulty State ---
    private float obstacleDensity = 1.0f;
    private float patternComplexity = 1.0f;
    private float coinDensity = 1.0f;

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

    private void OnEnable()
    {
        AdaptiveDifficultyManager.OnObstacleDensityChanged += (value) => obstacleDensity *= value;
        AdaptiveDifficultyManager.OnPatternComplexityChanged += (value) => patternComplexity *= value;
        AdaptiveDifficultyManager.OnCoinDensityChanged += (value) => coinDensity *= value;
    }

    private void OnDisable()
    {
        AdaptiveDifficultyManager.OnObstacleDensityChanged -= (value) => obstacleDensity *= value;
        AdaptiveDifficultyManager.OnPatternComplexityChanged -= (value) => patternComplexity *= value;
        AdaptiveDifficultyManager.OnCoinDensityChanged -= (value) => coinDensity *= value;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void OnObstacleSpawned()
    {
        if (ServiceLocator.IsRegistered<AdaptiveDifficultyManager>())
        {
            ServiceLocator.Get<AdaptiveDifficultyManager>().SetLastObstacleTime();
        }
    }

    public float GetObstacleDensity()
    {
        return obstacleDensity;
    }

    public float GetPatternComplexity()
    {
        return patternComplexity;
    }

    public float GetCoinDensity()
    {
        return coinDensity;
    }
}
