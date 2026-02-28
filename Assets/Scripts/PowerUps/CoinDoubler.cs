
using UnityEngine;

public class CoinDoubler : MonoBehaviour
{
    private void OnEnable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetCoinMultiplier(2);
        }
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetCoinMultiplier(1);
        }
    }
}
