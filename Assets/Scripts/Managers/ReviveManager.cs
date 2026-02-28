
using UnityEngine;
using System;

/// <summary>
/// Authoritative manager for all revive-related logic.
/// It validates and processes revive attempts, acting as the single point of entry for reviving the player.
/// </summary>
public class ReviveManager : MonoBehaviour
{
    public enum ReviveType
    {
        Gems,
        Ad,
        Token
    }

    [Header("Revive Configuration")]
    [SerializeField] private int gemCost = 10;
    
    // Dependencies - these would be fetched from a ServiceLocator
    private CurrencyManager currencyManager;
    private ReviveTokenManager reviveTokenManager;
    private AdMobManager adMobManager; // Assuming an ad manager exists

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        // These services must be registered before ReviveManager starts
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        reviveTokenManager = ServiceLocator.Get<ReviveTokenManager>();
        adMobManager = ServiceLocator.Get<AdMobManager>();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<ReviveManager>();
    }

    /// <summary>
    /// Checks if the player is eligible for a revive in the current run.
    /// </summary>
    public bool CanRevive(RunSessionData runSessionData)
    {
        // The primary rule: only one revive per run.
        return !runSessionData.hasRevived;
    }

    /// <summary>
    /// Initiates the revive process based on the selected type.
    /// </summary>
    public void StartRevive(RunSessionData runSessionData, PlayerController playerController, ReviveType reviveType, Action onReviveComplete)
    {
        // Double-check eligibility to prevent exploits
        if (!CanRevive(runSessionData))
        {
            GameStateManager.CurrentState = GameState.EndOfRun;
            return;
        }

        GameStateManager.CurrentState = GameState.Reviving;

        switch (reviveType)
        {
            case ReviveType.Gems:
                ProcessGemRevive(playerController, onReviveComplete);
                break;
            case ReviveType.Ad:
                ProcessAdRevive(playerController, onReviveComplete);
                break;
            case ReviveType.Token:
                ProcessTokenRevive(playerController, onReviveComplete);
                break;
            default:
                // Unknown revive type, fail safely
                GameStateManager.CurrentState = GameState.EndOfRun;
                break;
        }
    }

    private void ProcessGemRevive(PlayerController playerController, Action onReviveComplete)
    {
        if (currencyManager.SpendGems(gemCost))
        {
            GrantRevive(playerController, onReviveComplete);
        }
        else
        {
            FailRevive();
        }
    }

    private void ProcessAdRevive(PlayerController playerController, Action onReviveComplete)
    {
        // Assuming the AdMobManager has a callback for success and failure
        adMobManager.ShowRewardedAd(
            () => GrantRevive(playerController, onReviveComplete), // Success
            () => FailRevive()                                   // Failure/Skip
        );
    }

    private void ProcessTokenRevive(PlayerController playerController, Action onReviveComplete)
    {
        if (reviveTokenManager.UseToken())
        {
            GrantRevive(playerController, onReviveComplete);
        }
        else
        {
            FailRevive();
        }
    }

    private void GrantRevive(PlayerController playerController, Action onReviveComplete)
    {
        playerController.Revive();
        onReviveComplete?.Invoke();
    }

    private void FailRevive()
    {
        // If the revive fails for any reason (not enough currency, ad skipped), end the run.
        GameStateManager.CurrentState = GameState.EndOfRun;
    }
}
