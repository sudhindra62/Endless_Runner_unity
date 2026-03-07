
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public static void SetHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScoreKey, score);
    }
}
