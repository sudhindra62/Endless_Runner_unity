
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore(int amount)
    {
        if (PowerupManager.Instance != null && PowerupManager.Instance.IsDoubleCoinsActive())
        {
            amount *= 2;
        }
        score += amount;
        // Update UI or other game elements with the new score
    }

    public int GetScore()
    {
        return score;
    }
}
