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

    /* -------------------------
     * Events
     * ------------------------- */
    public static event Action OnReviveSuccess;
    public static event Action OnReviveFailed;

    /* -------------------------
     * Configuration
     * ------------------------- */
    [Header("Configuration")]
    [SerializeField] private int reviveGemCost = 50;
    [SerializeField] private float invincibilityDuration = 3f;

    private bool hasRevivedThisRun = false;

    /* -------------------------
     * Lifecycle
     * ------------------------- */
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

    /* -------------------------
     * Run State
     * ------------------------- */
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

    /* -------------------------
     * Revive Entry Points
     * ------------------------- */

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

    /* -------------------------
     * Core Revive Logic
     * ------------------------- */
    public void RevivePlayer()
    {
        if (!CanRevive())
        {
            OnReviveFailed?.Invoke();
            return;
        }

        hasRevivedThisRun = true;

        // Resume time
        Time.timeScale = 1f;

        // Restore player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerDeathHandler death = player.GetComponent<PlayerDeathHandler>();
            if (death != null)
                death.ResetDeath();

            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.Resume();
        }

        OnReviveSuccess?.Invoke();

        // Resume game flow
        GameFlowController.Instance?.ResumeAfterRevive();
    }
}
