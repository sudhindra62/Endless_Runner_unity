
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    public int Multiplier = 2;

    private void OnEnable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScoreMultiplier(Multiplier);
        }
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScoreMultiplier(1);
        }
    }
}
