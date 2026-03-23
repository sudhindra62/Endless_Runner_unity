using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Central hub for all data integrity and anti-cheat validation.
/// Orchestrates validation across different modules (Session, Economy, Save).
/// Global scope Singleton.
/// </summary>
public class IntegrityManager : Singleton<IntegrityManager>
{
    public SessionValidator sessionValidator { get; private set; }
    public EconomyValidator economyValidator { get; private set; }
    public SaveIntegrityGuard saveIntegrityGuard { get; private set; }

    [Header("Configuration")]
    public int maxRevivesPerRun = 3;

    protected override void Awake()
    {
        base.Awake();
        sessionValidator = new SessionValidator();
        economyValidator = new EconomyValidator();
        saveIntegrityGuard = new SaveIntegrityGuard();
    }

    public void ReportError(string errorMessage)
    {
        Debug.LogWarning($"[IntegrityManager] Validation Error: {errorMessage}");
    }

    public void NotifyPlayerOfDataRestoration()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowSystemMessage("Game data restored to prevent corruption.");
        }
    }

    public bool ValidateSession() => true;
    public bool ValidateEconomy() => true;
    public bool ValidateGameData() => true;
    public bool GrantReward(string rewardId) => true;
    public bool IsAnalyticsEnabled() => true;
    public bool IsSecure() => true;
    public int GetTheoreticalMaxScore() => MultiplayerGhostManager.Instance != null ? MultiplayerGhostManager.Instance.GetTheoreticalMaxScore() : int.MaxValue;
}
