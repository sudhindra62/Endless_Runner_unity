using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Scoring Settings")]
    public float distanceScoreMultiplier = 1f;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    public int CurrentScore { get; private set; }
    private int bestScore;

    private const string BestScoreKey = "BestScore";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }
    
    public void RegisterPlayer(PlayerController player)
    {
        // This method can be used for future logic if the ScoreManager needs to know about the player directly.
    }

    public void AddScoreFromDistance(float distance)
    {
        int points = Mathf.FloorToInt(distance * distanceScoreMultiplier);
        AddPoints(points);
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0) return;

        int multiplier = 1;
        if (StyleManager.Instance != null)
        {
            multiplier = StyleManager.Instance.ScoreMultiplier;
        }

        CurrentScore += amount * multiplier;
        UpdateScoreUI();
    }
    
    public int GetBestScore()
    {
        return bestScore;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreUI();
    }

    public void SaveBestScore()
    {
        if (CurrentScore > bestScore)
        {
            bestScore = CurrentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + CurrentScore;
        }
    }
}
