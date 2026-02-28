
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

    private void Awake()
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

    public void GrantCoinReward(int amount)
    {
        DataManager.Instance.Coins += amount;
    }

    public void GrantGemReward(int amount)
    {
        DataManager.Instance.Gems += amount;
    }
}
