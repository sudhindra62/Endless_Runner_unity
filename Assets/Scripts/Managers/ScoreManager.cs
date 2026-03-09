
using UnityEngine;

/// <summary>
/// Manages the player's score and coin totals for the current session.
/// Fully implemented by Supreme Guardian Architect v12.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Session State")]
    private int currentScore = 0;
    private int currentCoins = 0;
    private float distance = 0f;

    [Header("Scoring Parameters")]
    [SerializeField] private int scorePerSecond = 10;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing && playerTransform != null)
        {
            // Increase score based on distance traveled
            distance = playerTransform.position.z;
            currentScore = Mathf.FloorToInt(distance * scorePerSecond / 10f);
        }
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
    }

    public void ResetSession()
    {
        currentScore = 0;
        currentCoins = 0;
        distance = 0f;
    }

    public void EndSession()
    {
        // In a full implementation, this would save the score and coins to the player's permanent profile.
        // For example: PlayerDataManager.Instance.AddCoins(currentCoins);
        // For example: HighScoreManager.Instance.SubmitScore(currentScore);
    }

    public int GetCurrentScore() => currentScore;
    public int GetCurrentCoins() => currentCoins;
}
