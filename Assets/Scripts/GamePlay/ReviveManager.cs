using UnityEngine;
using System;

/// <summary>
/// Manages the logic for the player revive system.
/// Handles gem, token, and ad-based revives.
/// Does NOT control UI directly.
/// </summary>
public class ReviveManager : MonoBehaviour
{
    public static ReviveManager Instance { get; private set; }

    public static event Action OnReviveSuccess;
    public static event Action OnReviveFailed;

    [Header("Configuration")]
    [SerializeField] private int reviveGemCost = 50;
    [SerializeField] private float invincibilityDuration = 3f;

    private bool hasRevivedThisRun = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetForNewRun()
    {
        hasRevivedThisRun = false;
    }

    public bool CanRevive()
    {
        return !hasRevivedThisRun;
    }

    public int GetReviveGemCost() => reviveGemCost;
    public float GetInvincibilityDuration() => invincibilityDuration;

    public void ReviveWithToken()
    {
        if (!CanRevive())
        {
            OnReviveFailed?.Invoke();
            return;
        }

        if (ReviveTokenManager.Instance != null &&
            ReviveTokenManager.Instance.UseToken())
        {
            RevivePlayer();
        }
        else
        {
            OnReviveFailed?.Invoke();
        }
    }

    public void ReviveWithGems()
    {
        if (!CanRevive())
        {
            OnReviveFailed?.Invoke();
            return;
        }

        if (CurrencyManager.Instance != null &&
            CurrencyManager.Instance.CanAfford(reviveGemCost, "gems"))
        {
            CurrencyManager.Instance.AddGems(-reviveGemCost);
            RevivePlayer();
        }
        else
        {
            OnReviveFailed?.Invoke();
        }
    }

    public void DeclineRevive()
    {
        Debug.Log("Revive declined by player");
        OnReviveFailed?.Invoke();
    }

    /// <summary>
    /// Core revive logic. This method is called after a revive option is successfully chosen.
    /// It updates the revive state, notifies other systems, and resumes the game flow.
    /// </summary>
    public void RevivePlayer()
    {
        if (!CanRevive())
        {
            OnReviveFailed?.Invoke();
            return;
        }

        hasRevivedThisRun = true;

        // The PlayerController is subscribed to this event and will handle its own state reset.
        OnReviveSuccess?.Invoke();

        // The GameFlowController will handle resuming time and the overall game state.
        GameFlowController.Instance?.ResumeAfterRevive();
    }
}
