
using UnityEngine;

/// <summary>
/// Manages persistent player data like best score and best time.
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    private const string BestScoreKey = "BestScore";
    private const string BestTimeKey = "BestTime";

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerDataManager>();
    }

    public void UpdateBestScore(int score)
    {
        int currentBest = PlayerPrefs.GetInt(BestScoreKey, 0);
        if (score > currentBest)
        {
            PlayerPrefs.SetInt(BestScoreKey, score);
            PlayerPrefs.Save();
        }
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    public void UpdateBestTime(float time)
    {
        float currentBest = PlayerPrefs.GetFloat(BestTimeKey, 0f);
        if (time > currentBest)
        {
            PlayerPrefs.SetFloat(BestTimeKey, time);
            PlayerPrefs.Save();
        }
    }

    public float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BestTimeKey, 0f);
    }
}
