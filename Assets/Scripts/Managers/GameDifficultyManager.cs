using UnityEngine;

/// <summary>
/// Manages the game's difficulty level based on player performance.
/// </summary>
public class GameDifficultyManager : MonoBehaviour
{
    public int currentDifficultyLevel = 1;
    public int maxDifficultyLevel = 10;
    public float scoreThresholdForLevelUp = 1000f;

    private float _scoreSinceLastLevelUp = 0;
    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = ServiceLocator.Get<ScoreManager>();
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreAdded += OnScoreGained;
        }
    }

    private void OnDestroy()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreAdded -= OnScoreGained;
        }
    }

    private void OnScoreGained(int score)
    {
        _scoreSinceLastLevelUp += score;
        if (_scoreSinceLastLevelUp >= scoreThresholdForLevelUp && currentDifficultyLevel < maxDifficultyLevel)
        {
            currentDifficultyLevel++;
            _scoreSinceLastLevelUp = 0;
            Debug.Log("Difficulty Increased to Level " + currentDifficultyLevel);
            // Broadcast an event or directly modify game parameters like speed, spawn rate, etc.
        }
    }
}
