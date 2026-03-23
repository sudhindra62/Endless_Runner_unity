using UnityEngine;
using System;

/// <summary>
/// Manages player revival logic (Gems, Ads, or Decline).
/// </summary>
public class ReviveManager : Singleton<ReviveManager>
{
    public static event Action OnPlayerRevived;
    public static event Action OnReviveSuccess;
    public static event Action OnReviveDeclined;

    [SerializeField] private int baseGemCost = 5;
    [SerializeField] private int adsWatchedThisRun = 0;
    private bool isPlayerReviving;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register(this);
    }

    public void ReviveWithGems()
    {
        isPlayerReviving = true;
        int cost = GetReviveCost();
        if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.SpendCurrency(CurrencyType.Gems, cost))
        {
            ExecuteRevive();
        }
        else
        {
            Debug.Log("Guardian Architect: Insufficient gems for revive.");
        }
    }

    public void ReviveWithAd()
    {
        // Integration with AdManager
        if (AdManager.Instance != null)
        {
            // Placeholder for Ad callback
            isPlayerReviving = true;
            ExecuteRevive();
            adsWatchedThisRun++;
        }
    }

    public void DeclineRevive()
    {
        isPlayerReviving = false;
        OnReviveDeclined?.Invoke();
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }

    private void ExecuteRevive()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
            // Additional logic to reposition player or reset state
        }
        isPlayerReviving = false;
        OnPlayerRevived?.Invoke();
        OnReviveSuccess?.Invoke();
    }

    public int GetReviveCost()
    {
        // Incremental cost logic could go here
        return baseGemCost;
    }

    public bool IsPlayerReviving() => isPlayerReviving;
}
