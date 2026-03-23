using UnityEngine;

/// <summary>
/// Architectural proxy for ScoreManager.
/// Provides a consistent naming interface for project-wide call-sites.
/// </summary>
public class ScoringManager : Singleton<ScoringManager>
{
    public static ScoringManager instance => Instance;
    public UnityEngine.UI.Text scoreText;

    public int Score => ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
    public int HighScore => ScoreManager.Instance != null ? ScoreManager.Instance.HighScore : 0;

    public void AddScore(int amount)
    {
        if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(amount);
    }

    public void ResetScore()
    {
        // ScoreManager.Instance.ResetScore() is private, but the instance property resets on state change.
    }

    // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
    
    public void AddScore(long amount)
    {
        AddScore((int)System.Math.Min(amount, int.MaxValue));
    }

    public bool IsHighScore(long score)
    {
        return (long)Score > score || (long)HighScore > score
            ? false
            : (long)score > (long)HighScore;
    }

    public long GetScoreAsLong() => (long)Score;
    
    public long GetHighScoreAsLong() => (long)HighScore;
}
